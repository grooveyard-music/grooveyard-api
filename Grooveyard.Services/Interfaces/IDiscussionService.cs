using Grooveyard.Domain.Entities;
using Grooveyard.Services.DTOs;

namespace Grooveyard.Services.Interfaces
{
    /// <summary>
    /// Methods for features that support apprenticeships such as Employers and Mentors
    /// </summary>
    public interface IDiscussionService
    {
        Task<List<DiscussionDto>> GetDiscussions();
        Task<DiscussionDto> GetDiscussion(string discussionId);
        Task<Discussion> CreateDiscussion(CreateDiscussionDto discussion);
        Task<bool> DeleteDiscussion(string discussionId);
        Task UpdateDiscussionDate(string discussionId);

        Task<bool> ToggleSubscription(string discussionId, string userId);
        Task<int> GetDiscussionCountForUser(string userId);
        Task<bool> IsSubscribedToDiscussion(string discussionId, string userId);
        Task<List<Genre>> GetAllGenres();
    }
}
