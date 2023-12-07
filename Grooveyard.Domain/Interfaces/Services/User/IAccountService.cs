using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Models.User;
using Microsoft.AspNetCore.Identity;




namespace Grooveyard.Domain.Interfaces.Services.User
{
    public interface IAccountService
    {
        Task<ServiceResult<IdentityUser>> RegisterUser(RegisterModel model);
        Task<ServiceResult<bool>> CheckEmailExists(string email);
        Task<ServiceResult<LoginResponse>> LoginUser(LoginModel model);
        Task<ServiceResult<UserDTO>> AuthenticateUser(string userId);
        Task<ServiceResult<ExternalLoginResponse>> ExternalLoginUser(ExternalLoginInfo info);
        Task<ServiceResult<bool>> RevokeToken(string currentUser);
        Task<ServiceResult<TokenResponse>> RefreshToken(string refreshToken);
        Task<ServiceResult<bool>> Logout(string userId);
    }
}
