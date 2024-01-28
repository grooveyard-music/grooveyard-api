using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserProfileService _profileService;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(IAccountService accountService, SignInManager<IdentityUser> signInManager, IUserProfileService profileService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            var result = await _accountService.RegisterUser(model);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _accountService.LoginUser(login);

            if (result.Success)
            {
                Response.Cookies.Append("JWT", result.Data.Tokens.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1) 
                });
                Response.Cookies.Append("RefreshToken", result.Data.Tokens.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
                Response.Cookies.Append("IsLoggedIn", "true", new CookieOptions
                {
                    HttpOnly = false, 
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                return Ok(result.Data.User);
            }

            return Unauthorized();
        }

        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("Error loading external login information.");
            }

            var result = await _accountService.ExternalLoginUser(info);
            if (result.Success)
            {
                Response.Cookies.Append("JWT", result.Data.Tokens.Token, new CookieOptions { HttpOnly = true });
                Response.Cookies.Append("RefreshToken", result.Data.Tokens.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true });
                Response.Cookies.Append("IsLoggedIn", "true", new CookieOptions
                {
                    HttpOnly = false, 
                    Secure = true,
                    SameSite = SameSiteMode.None, 
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
                return Ok();
            }

            return Unauthorized();
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string userId)
        {
            var revokeResult = await _accountService.Logout(userId);

            if (revokeResult.Success)
            {
                Response.Cookies.Delete("JWT");
                Response.Cookies.Delete("RefreshToken");
                Response.Cookies.Delete("IsLoggedIn");
         
                return Ok();
            }

            return BadRequest("Error revoking refresh token.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Retrieve the refresh token from the cookie
            var refreshTokenFromCookie = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                return BadRequest("No refresh token provided.");
            }
            var result = await _accountService.RefreshToken(refreshTokenFromCookie);

            if (result.Success)
            {
                Response.Cookies.Append("JWT", result.Data.Token, new CookieOptions { HttpOnly = true, Secure = true });
                Response.Cookies.Append("RefreshToken", result.Data.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true });
                Response.Cookies.Append("IsLoggedIn", "true", new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(string userId)
        {
            var result = await _accountService.RevokeToken(userId);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var result = await _accountService.CheckEmailExists(email);

            return Ok(result.Data);
        }

        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var user = User;

            if (user.Identity?.IsAuthenticated ?? false)
            {
                var authUser = new UserDTO
                {
                    UserName = user.FindFirst(ClaimTypes.Name)?.Value,
                    Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Avatar = await _profileService.GetUserAvatar(user.FindFirstValue(ClaimTypes.NameIdentifier))
                };
                var roles = user.FindAll(ClaimTypes.Role);
                authUser.Roles = new List<string>();
                foreach (var role in roles)
                {
                    authUser.Roles.Add(role.Value);
                }

                return Ok(authUser);
            }

            return Unauthorized();
        }



    }
}