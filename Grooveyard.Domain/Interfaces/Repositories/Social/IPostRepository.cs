using Grooveyard.Domain.Models.Media;
using Grooveyard.Domain.Models.Social;

namespace Grooveyard.Domain.Interfaces.Repositories.Social
{
    public interface IPostRepository
    {
        Task<Post> GetPostAsync(string postId);
        Task<List<Post>> GetPostsAsync(string discussionId);
        Task<Post> CreatePostAsync(Post post);
        //   Task UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(Post post);
        Task<int> GetPostCountForUserAsync(string userId);
        Task<int> GetCommentCountForUserAsync(string userId);
        Task<Genre> GetOrCreateGenre(string genreName);
        Task<Comment> CreateCommentAsync(Comment comment);

        Task<List<Comment>> GetCommentsAsync(string postId);
        Task<Like> ToggleLikeAsync(Like like);
    }
}
