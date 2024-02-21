using Grooveyard.Domain.Entities;
using Grooveyard.Services.DTOs;

namespace Grooveyard.Services.Interfaces
{
    public interface IPostService
    {

        //  Task<Post> GetPostAsync(string postId);
        Task<List<PostDto>> GetPostsAsync(string discussionId);
        Task<PostDto> GetPostAndCommentsAsync(string p);
        Task<PostDto> CreatePostAsync(CreatePostDto postDto, string userId);

        //   Task<Post> UpdatePostAsync(string postId, UpdatePostDto postDto);
        Task<bool> DeletePostAsync(string postId);
        Task<bool> DeleteCommentAsync(string commentId);
        Task<Comment> CreateCommentAsync(CreateCommentDto comment);
        Task<List<CommentDto>> GetCommentsAsync(string postId);
        Task<LikeDto> TogglePostLikeAsync(ToggleLikeDto likeDto);
        Task<LikeDto> ToggleCommentLikeAsync(ToggleLikeDto likeDto);

    }
}
