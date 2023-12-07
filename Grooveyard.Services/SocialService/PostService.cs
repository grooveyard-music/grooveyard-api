using AutoMapper;
using Grooveyard.Domain.DTO.Social;
using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.Interfaces.Services.Social;
using Grooveyard.Domain.Models.Social;
using Grooveyard.Domain.Interfaces.Repositories.Social;
using Microsoft.Extensions.Logging;

namespace Grooveyard.Services.SocialSercice
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IDiscussionService _discussionService;

        private readonly ILogger<PostService> _logger;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository, ILogger<PostService> logger, IDiscussionService discussionService, IMapper mapper)
        {
            _postRepository = postRepository;
            _discussionService = discussionService;
            _logger = logger;
            _mapper = mapper;

        }
        public async Task<List<PostDto>> GetPostsAsync(string discussionId)
        {
            var posts = await _postRepository.GetPostsAsync(discussionId);

            var postDtoList = posts.Select(d => new PostDto
            {
                Id = d.Id,
                Title = d.Title,
                Content = d.Content,
                CreatedAt = d.CreatedAt,
                TotalComments = d.Comments.Count,
                TotalLikes = d.Likes.Count,
                Comments = d.Comments
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new CommentDto
                {
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                }).ToList()

            }).ToList();

            return postDtoList;
        }
        public async Task<PostDto> CreatePostAsync(CreatePostDto postDto, string userId)
        {
            try
            {
                var newPost = _mapper.Map<Post>(postDto);

                newPost.Id = Guid.NewGuid().ToString();
                newPost.CreatedAt = DateTime.Now;
                newPost.UserId = userId;

                var postToAdd = await CreateByPostType(newPost, postDto);

                var post =  await _postRepository.CreatePostAsync(postToAdd); 

                await _discussionService.UpdateDiscussionDate(post.DiscussionId);

                var addedPost = _mapper.Map<PostDto>(post);

                return addedPost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating a post for user {userId}");
                throw; 
            }
        }

        public async Task<int> GetPostCountForUser(string userId)
        {
            try
            {
             var postCount =  await _postRepository.GetPostCountForUserAsync(userId);

             return postCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting post count for user {userId}");
                throw;
            }
        }

        public async Task<int> GetCommentCountForUser(string userId)
        {
            try
            {
                var commentCount = await _postRepository.GetCommentCountForUserAsync(userId);

                return commentCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting post count for user {userId}");
                throw;
            }
        }
        public async Task<bool> DeletePostAsync(string postId)
        {
            var postToDelete = _postRepository.GetPostAsync(postId).Result;

            var isDeleted = await _postRepository.DeletePostAsync(postToDelete);

            return isDeleted;
        }

        public async Task<List<CommentDto>> GetCommentsAsync(string postId)
        {
            var comments = await _postRepository.GetCommentsAsync(postId);

            var commentDtoList = comments.Select(d => new CommentDto
            {
                Id = d.Id,
                Content = d.Content,
                CreatedAt = d.CreatedAt,
                CreatedById = d.UserId
            }).ToList();

            return commentDtoList;
        }

        public async Task<Comment> CreateCommentAsync(CreateCommentDto comment)
        {
            var newComment = _mapper.Map<Comment>(comment);
            newComment.CreatedAt = DateTime.Now;
            newComment.Id = Guid.NewGuid().ToString();
            var commentAdded = await _postRepository.CreateCommentAsync(newComment);

            return commentAdded;
        }

        public async Task<Like> TogglePostLikeAsync(ToggleLikeDto likeDto)
        {
  
            var like = new Like
            {   Id = Guid.NewGuid().ToString(),
                UserId = likeDto.UserId,
                PostId = likeDto.EntityId,
                CreatedAt = DateTime.Now
            };

            return await _postRepository.ToggleLikeAsync(like);
        }

        public async Task<Like> ToggleCommentLikeAsync(ToggleLikeDto likeDto)
        { 
            var like = new Like
            {
                UserId = likeDto.UserId,
                CommentId = likeDto.EntityId,
                CreatedAt = DateTime.Now
            };

            return await _postRepository.ToggleLikeAsync(like);
        }

        #region Private Methods
        private async Task<Post> CreateByPostType(Post post, CreatePostDto createPostDto)
        {
            switch (post.Type)
            {
                case PostType.Track:
                    if (createPostDto.EntityId == null)
                    {
                        return post;
                    }             
      
                    break;
                case PostType.Mix:
                    if (createPostDto.EntityId == null)
                    {
                        return post;
                    }

      
                    break;
            }

            return post;
        }

   

        #endregion


    }
}
