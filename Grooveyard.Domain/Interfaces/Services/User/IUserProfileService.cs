using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Models.User;

namespace Grooveyard.Domain.Interfaces.Services.User
{

    public interface IUserProfileService
    {
        Task<UserProfile> CreateUserProfile(string userId);
        Task<UserProfileDto> UpdateUserProfile(UpdateUserProfileDto userProfile);
        Task<UserProfileDto> GetUserProfile(string userId);
        Task<string> GetUserAvatar(string userId);
        Task<List<UserProfileDto>> GetUserProfilesByIds(List<string> userIds);
        Task<bool> CheckDisplayName(string displayName);
    }
}
