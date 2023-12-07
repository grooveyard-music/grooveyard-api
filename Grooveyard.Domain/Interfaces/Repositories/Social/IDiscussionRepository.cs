using Grooveyard.Domain.Models.Media;
using Grooveyard.Domain.Models.Social;

namespace Grooveyard.Domain.Interfaces.Repositories.Social
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
        Task<Discussion> SubscribeToDiscussion(string discussionId, string userId);
        Task<List<Genre>> GetAllGenres();
    }
}
