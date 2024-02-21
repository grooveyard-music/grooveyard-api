using Grooveyard.Domain.Entities;

namespace Grooveyard.Infrastructure.Data.Repositories.Interfaces
{
    public interface IPostRepository
    {
   
        Task<List<Post>> GetPostsAsync(string discussionId);
        Task<Post> GetPostAndCommentsAsync(string discussionId);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> GetPostAsync(string postId);
        Task<Comment> GetCommentAsync(string commentId);
        //   Task UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(Post post);
        Task<bool> DeleteCommentAsync(Comment comment);
        Task<Genre> GetOrCreateGenre(string genreName);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<int> GetPostCountForUserAsync(string userId);
        Task<int> GetCommentCountForUserAsync(string userId);
        Task<List<Comment>> GetCommentsAsync(string postId);
        Task<Like> ToggleLikeAsync(Like like);
    }
}
