using Grooveyard.Domain.Entities;
using Grooveyard.Services.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Grooveyard.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<IdentityUser>> RegisterUser(RegisterModel model);
        Task<ServiceResult<string>> CheckEmailExists(string email);
        Task<ServiceResult<LoginResponse>> LoginUser(LoginModel model);
        Task<ServiceResult<UserDTO>> AuthenticateUser(string userId);
        Task<ServiceResult<ExternalLoginResponse>> ExternalLoginUser(ExternalLoginInfo info);
        Task<ServiceResult<bool>> RevokeToken(string currentUser);
        Task<ServiceResult<TokenResponse>> RefreshToken(string refreshToken);
        Task<ServiceResult<bool>> Logout(string userId);
        Task<List<UserDTO>> GetUsersByIds(List<string> userIds);
        Task<UserDTO> GetUserById(string userId);
    }
}
