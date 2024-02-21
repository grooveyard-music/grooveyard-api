using AutoMapper;
using Microsoft.Extensions.Logging;
using Grooveyard.Services.Interfaces;
using Grooveyard.Domain.Entities;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Grooveyard.Services.DTOs;

namespace Grooveyard.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IDiscussionService _discussionService;
        private readonly IUserRepository _userRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly ILogger<PostService> _logger;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository, ILogger<PostService> logger, IDiscussionService discussionService, IMapper mapper, IUserRepository userRepostory, IMediaRepository mediaRepository)
        {
            _postRepository = postRepository;
            _discussionService = discussionService;
            _userRepository = userRepostory;
            _logger = logger;
            _mapper = mapper;
            _mediaRepository = mediaRepository;
        }
        public async Task<List<PostDto>> GetPostsAsync(string discussionId)
        {
            var fetchPosts = await _postRepository.GetPostsAsync(discussionId);
            
            var postDtoList = new List<PostDto>();

            foreach (var d in fetchPosts)
            {
                var user = await _userRepository.GetUserProfile(d.UserId);
                var comments = new List<CommentDto>();
              
                foreach (var c in d.Comments.OrderByDescending(c => c.CreatedAt).Take(5))
                {
                    var commentUser = await _userRepository.GetUserProfile(c.UserId);
                    comments.Add(new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        CreatedById = c.UserId,
                        CreatedByUsername = commentUser?.DisplayName ?? "User Deleted",
                        CreatedByAvatar = commentUser?.AvatarUrl,
                        TotalLikes = c.Likes.Count(),
                    });
                }
                var getTrack = await _mediaRepository.GetTrackByIdAsync(d.TrackId); 
                var newTrackDto = getTrack != null ? _mapper.Map<TrackDto>(getTrack) : null; 

                postDtoList.Add(new PostDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Content = d.Content,
                    CreatedAt = d.CreatedAt,
                    TotalComments = d.Comments.Count(),
                    TotalLikes = d.Likes.Count,
                    CreatedById = user?.UserId ?? null,
                    CreatedByUsername = user?.DisplayName ?? "User Deleted",
                    CreatedByAvatar = user?.AvatarUrl,
                    Track = newTrackDto,
                    Comments = comments
                });
            }

            return postDtoList;
        }


        public async Task<PostDto> GetPostAndCommentsAsync(string postId)
        {
            var fetchPost = await _postRepository.GetPostAndCommentsAsync(postId);
            var postDto = new PostDto();

 
                var user = await _userRepository.GetUserProfile(fetchPost.UserId);
                var comments = new List<CommentDto>();

                foreach (var c in fetchPost.Comments.OrderByDescending(c => c.CreatedAt))
                {
                    var commentUser = await _userRepository.GetUserProfile(c.UserId);
                    comments.Add(new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        CreatedById = c.UserId,
                        CreatedByUsername = commentUser?.DisplayName ?? "User Deleted",
                        CreatedByAvatar = commentUser?.AvatarUrl,
                        TotalLikes = c.Likes.Count(),
                    });
                }
            var getTrack = await _mediaRepository.GetTrackByIdAsync(fetchPost.TrackId); // Ensure you're using the correct ID
            var newTrackDto = getTrack != null ? _mapper.Map<TrackDto>(getTrack) : null;
            postDto = new PostDto
            {
                Id = fetchPost.Id,
                Title = fetchPost.Title,
                Content = fetchPost.Content,
                CreatedAt = fetchPost.CreatedAt,
                TotalComments = fetchPost.Comments.Count(),
                TotalLikes = fetchPost.Likes.Count,
                CreatedById = user?.UserId ?? null,
                CreatedByUsername = user?.DisplayName ?? "User Deleted",
                CreatedByAvatar = user?.AvatarUrl,
                Track = newTrackDto,
                Comments = comments
            };

           

            return postDto;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto postDto, string userId)
        {
            try
            {
                var newPost = _mapper.Map<Post>(postDto);

                newPost.Id = Guid.NewGuid().ToString();
                newPost.CreatedAt = DateTime.Now;
                newPost.UserId = userId;

                var post = await _postRepository.CreatePostAsync(newPost);

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

        public async Task<bool> DeletePostAsync(string postId)
        {
            var postToDelete = await _postRepository.GetPostAsync(postId);

            var isDeleted = await _postRepository.DeletePostAsync(postToDelete);

            return isDeleted;
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            var postToDelete = await _postRepository.GetCommentAsync(commentId);

            var isDeleted = await _postRepository.DeleteCommentAsync(postToDelete);

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
                CreatedById = d.UserId,
                TotalLikes = d.Likes.Count,

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

        public async Task<LikeDto> TogglePostLikeAsync(ToggleLikeDto likeDto)
        {

            var like = new Like
            {
                Id = Guid.NewGuid().ToString(),
                UserId = likeDto.UserId,
                PostId = likeDto.EntityId,
                CreatedAt = DateTime.Now
            };

            var likeAdded = await _postRepository.ToggleLikeAsync(like);

            var mappedLiked = _mapper.Map<LikeDto>(likeAdded);

            return mappedLiked;
        }

        public async Task<LikeDto> ToggleCommentLikeAsync(ToggleLikeDto likeDto)
        {
            var like = new Like
            {
                Id = Guid.NewGuid().ToString(),
                UserId = likeDto.UserId,
                CommentId = likeDto.EntityId,
                CreatedAt = DateTime.Now
            };

           var likeAdded =  await _postRepository.ToggleLikeAsync(like);

            var mappedLiked = _mapper.Map<LikeDto>(likeAdded);

            return mappedLiked;
        }

        #region Private Methods




        #endregion


    }
}
