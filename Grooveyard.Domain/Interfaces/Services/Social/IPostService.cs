using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.DTO.Social;
using Grooveyard.Domain.Models.Social;

namespace Grooveyard.Domain.Interfaces.Services.Social
{
    public interface IPostService
    {

        //  Task<Post> GetPostAsync(string postId);
        Task<List<PostDto>> GetPostsAsync(string discussionId);
        Task<PostDto> CreatePostAsync(CreatePostDto postDto, string userId);

        //   Task<Post> UpdatePostAsync(string postId, UpdatePostDto postDto);
        Task<bool> DeletePostAsync(string postId);

        Task<int> GetPostCountForUser(string userId);
        Task<int> GetCommentCountForUser(string userId);
        Task<Comment> CreateCommentAsync(CreateCommentDto comment);
        Task<List<CommentDto>> GetCommentsAsync(string postId);
        Task<Like> TogglePostLikeAsync(ToggleLikeDto likeDto);
        Task<Like> ToggleCommentLikeAsync(ToggleLikeDto likeDto);

    }
}
