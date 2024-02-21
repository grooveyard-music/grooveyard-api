using Grooveyard.Domain.Entities;

namespace Grooveyard.Infrastructure.Data.Repositories.Interfaces
{
    public interface IDiscussionRepository
    {
        Task<List<Discussion>> GetDiscussions();

        Task<Discussion> GetDiscussion(string discussionId);
        Task<bool> DeleteDiscussion(Discussion discussion);
        Task<Genre> GetOrCreateGenre(string genreName);
        Task<Discussion> CreateDiscussion(Discussion discussion);
        Task<bool> UpdateDiscussionDate(Discussion discussion);
        Task<int> GetDiscussionCountForUserAsync(string userId);
        Task<bool> ToggleSubscription(string discussionId, string userId);
        Task<bool> GetUserSubscription(string discussionId, string userId);
        Task<List<Genre>> GetAllGenres();
        Task<int> GetDiscussionSubscriptionCount(string discussionId);
    }
}
