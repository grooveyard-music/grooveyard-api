
using Grooveyard.Domain.Entities;
using Grooveyard.Services.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;


namespace Grooveyard.Services.Interfaces
{

    public interface IUserProfileService
    {
        Task<UserProfile> CreateUserProfile(string userId, string userName);
        Task<UserProfileDto> UpdateUserProfile(UpdateUserProfileDto userProfile);
        Task<UserProfileDto> GetUserProfile(string userId);
        Task<string> GetUserAvatar(string userId);
        Task<List<UserProfileDto>> GetUserProfilesByIds(List<string> userIds);
        Task<bool> CheckDisplayName(string displayName);
        Task<List<TrackDto>> GetUserMusicboxAsync(string userId);
        Task<List<TrackDto>> SearchUserMusicboxAsync(string userId, string searchTerm);
        Task<bool> UpdateUserProfileAvatar(string userId, IFormFile imageFile);
        Task<bool> UpdateUserProfileCover(string userId, IFormFile imageFile);
    }
}
