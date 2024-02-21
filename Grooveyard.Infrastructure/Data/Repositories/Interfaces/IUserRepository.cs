using Grooveyard.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Grooveyard.Infrastructure.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserProfile> CreateUserProfile(UserProfile userProfile);
        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);
        Task<UserProfile> GetUserProfile(string userId);
        Task<List<UserProfile>> GetUserProfilesByIds(List<string> userId);
        Task<List<IdentityUser>> GetUsersByIds(List<string> userIds);
        Task<IdentityUser> GetUserById(string userId);
        Task StoreRefreshToken(RefreshToken refreshToken);
        Task RevokeRefreshToken(string userId);
        Task UpdateRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(string userId);
        Task<RefreshToken> GetRefreshTokenByToken(string refreshToken);

        Task<bool> CheckDisplayName(string displayName);
    }
}
