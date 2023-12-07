﻿using Grooveyard.Domain.Models.User;

namespace Grooveyard.Domain.Interfaces.Repositories.User
{
    public interface IUserRepository
    {
        Task<UserProfile> CreateUserProfile(UserProfile userProfile);
        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);
        Task<UserProfile> GetUserProfile(string userId);
        Task<List<UserProfile>> GetUserProfilesByIds(List<string> userId);
        Task StoreRefreshToken(RefreshToken refreshToken);
        Task RevokeRefreshToken(string userId);
        Task UpdateRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(string userId);
        Task<RefreshToken> GetRefreshTokenByToken(string refreshToken);

        Task<bool> CheckDisplayName(string displayName);
    }
}
