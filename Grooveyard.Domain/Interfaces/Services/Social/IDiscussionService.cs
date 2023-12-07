using Grooveyard.Domain.DTO.Social;
using Grooveyard.Domain.Models.Media;
using Grooveyard.Domain.Models.Social;

namespace Grooveyard.Domain.Interfaces.Services.Social
{
    /// <summary>
    /// Methods for features that support apprenticeships such as Employers and Mentors
    /// </summary>
    public interface IDiscussionService
    {
        Task<List<DiscussionDto>> GetDiscussions();
        Task<Discussion> CreateDiscussion(CreateDiscussionDto discussion);
        Task<bool> DeleteDiscussion(string discussionId);
        Task UpdateDiscussionDate(string discussionId);

        Task<bool> SubscribeToDiscussion(string discussionId, string userId);
        Task<int> GetDiscussionCountForUser(string userId);

        Task<List<Genre>> GetAllGenres();
    }
}
