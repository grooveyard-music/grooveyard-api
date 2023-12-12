using AutoMapper;
using Grooveyard.Domain.DTO.Social;
using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Repositories.User;
using Grooveyard.Domain.Interfaces.Services.User;
using Grooveyard.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Grooveyard.Services.UserService
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _repository;
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountService(IConfiguration configuration, IUserRepository repository, IUserProfileService userProfileService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userProfileService = userProfileService;
            _mapper = mapper;

        }
        public async Task<ServiceResult<IdentityUser>> RegisterUser(RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var roleCheck = await _roleManager.RoleExistsAsync("User");
                if (!roleCheck)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(user, "User");
                await _userProfileService.CreateUserProfile(user.Id);
                return new ServiceResult<IdentityUser> { Success = true, Data = user, };
            }
            var errors = result.Errors.Select(e => e.Description);

            return new ServiceResult<IdentityUser> { Success = false, Errors = errors };
        }

        public async Task<ServiceResult<LoginResponse>> LoginUser(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    var token = await GenerateJwtToken(user);
                    var refreshToken = GenerateRefreshToken(user.Id);
                    await _repository.StoreRefreshToken(refreshToken);

                    var userDto = new UserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Roles = await _userManager.GetRolesAsync(user),
                        Avatar = await _userProfileService.GetUserAvatar(user.Id)
                    };

                    var response = new LoginResponse
                    {
                        Tokens = new TokenResponse
                        {
                            Token = token,
                            RefreshToken = refreshToken.Token
                        },
                        User = userDto
                    };

                    return new ServiceResult<LoginResponse> { Success = true, Data = response };
                }
            }
            return new ServiceResult<LoginResponse> { Success = false };

        }

        public async Task<ServiceResult<ExternalLoginResponse>> ExternalLoginUser(ExternalLoginInfo info)
        {
            bool isNewUser = false;
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                isNewUser = true;
                user = new IdentityUser { UserName = info.Principal.FindFirstValue(ClaimTypes.Email), Email = info.Principal.FindFirstValue(ClaimTypes.Email) };
                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    return new ServiceResult<ExternalLoginResponse> { Success = false, Errors = createUserResult.Errors.Select(e => e.Description) };
                }

                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    return new ServiceResult<ExternalLoginResponse> { Success = false, Errors = addLoginResult.Errors.Select(e => e.Description) };
                }

                await _userProfileService.CreateUserProfile(user.Id);

            }
            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id);
            await _repository.StoreRefreshToken(refreshToken);

            var userDto = new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user),
                Avatar = await _userProfileService.GetUserAvatar(user.Id)
            };

            var response = new ExternalLoginResponse
            {
                Tokens = new TokenResponse
                {
                    Token = token,
                    RefreshToken = refreshToken.Token
                },
                User = userDto,
                IsNewUser = isNewUser
            };

            return new ServiceResult<ExternalLoginResponse> { Success = true, Data = response };
        }


        public async Task<ServiceResult<UserDTO>> AuthenticateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles,
                    Avatar = await _userProfileService.GetUserAvatar(user.Id)
                };
                return new ServiceResult<UserDTO> { Success = true, Data = userDto };
            }
            return new ServiceResult<UserDTO> { Success = false };
        }

        public async Task<ServiceResult<TokenResponse>> RefreshToken(string refreshToken)
        {
            var storedRefreshToken = await _repository.GetRefreshTokenByToken(refreshToken);

            if (storedRefreshToken == null || storedRefreshToken.Revoked || storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
            {
                return new ServiceResult<TokenResponse> { Success = false, Errors = new List<string> { "Invalid or expired refresh token." } };
            }

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
            if (user == null)
            {
                return new ServiceResult<TokenResponse> { Success = false, Errors = new List<string> { "User not found." } };
            }

            // Mark old token as revoked
            storedRefreshToken.Revoked = true;
            await _repository.UpdateRefreshToken(storedRefreshToken);

            var jwtToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.Id);
            await _repository.StoreRefreshToken(newRefreshToken);

            var tokens = new TokenResponse
            {
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token
            };

            return new ServiceResult<TokenResponse> { Success = true, Data = tokens };
        }

        public async Task<ServiceResult<bool>> Logout(string userId)
        {

            var currentRefreshToken = await _repository.GetRefreshToken(userId);
            if (currentRefreshToken != null)
            {
                currentRefreshToken.Revoked = true;
                await _repository.UpdateRefreshToken(currentRefreshToken);
            }

            return new ServiceResult<bool> { Success = true, Data = true };
        }

        public async Task<ServiceResult<bool>> RevokeToken(string userId)
        {
            await _repository.RevokeRefreshToken(userId);
            return new ServiceResult<bool> { Success = true, Data = true };
        }

        public async Task<ServiceResult<string>> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return new ServiceResult<string> { Success = true, Data = user.UserName };
            }

            return new ServiceResult<string> { Success = false, Data = null };
        }

        public async Task<List<UserDTO>> GetUsersByIds(List<string> userIds)
        {
            var users = await _repository.GetUsersByIds(userIds);

            var userDtos = users.Select(d =>
            {
                return new UserDTO
                {
                    Id = d.Id,
                    UserName = d.UserName,
                    Email = d.NormalizedEmail,
                };
            }).ToList();


            return userDtos;
        }



        #region private methods
        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(5),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomNumber),
                ExpirationDate = DateTime.UtcNow.AddDays(30)
            };
        }

        #endregion

    }
}