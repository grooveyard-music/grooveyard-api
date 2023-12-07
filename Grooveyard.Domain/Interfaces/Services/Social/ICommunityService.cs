using Grooveyard.Domain.DTO.User;

namespace Grooveyard.Domain.Interfaces.Services.Social
{
    public interface ICommunityService
    {

        Task<UserActivityDto> GetUserCommunityActivity(string userId);


    }
}
