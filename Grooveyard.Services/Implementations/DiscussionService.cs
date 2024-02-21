using AutoMapper;
using Grooveyard.Domain.Entities;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Grooveyard.Services.DTOs;
using Grooveyard.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Grooveyard.Services.Implementations
{
    public class DiscussionService : IDiscussionService
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DiscussionService> _logger;
        private readonly IMapper _mapper;
        public DiscussionService(IDiscussionRepository discussionRepository, IUserRepository userRepository,
            IPostRepository postRepository, ILogger<DiscussionService> logger, IMapper mapper)
        {
            _discussionRepository = discussionRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<DiscussionDto>> GetDiscussions()
        {
            var activeDiscussions = await _discussionRepository.GetDiscussions();
            var userIds = activeDiscussions.Select(d => d.UserId).Distinct().ToList();

            // Ensure we have a non-null list of users
            var users = await _userRepository.GetUserProfilesByIds(userIds);

            List<DiscussionDto> discussions = new List<DiscussionDto>();

            foreach (var d in activeDiscussions)
            {
                var user = users.FirstOrDefault(u => u.UserId == d.UserId);
                var discussionPosts = await _postRepository.GetPostsAsync(d.Id);
                var discussionSubscriptions = await _discussionRepository.GetDiscussionSubscriptionCount(d.Id);

                discussions.Add(new DiscussionDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    CreatedById = user?.UserId ?? null,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Genres = d.Genres.Select(g => g.Name).ToList(),
                    CreatedByUsername = user?.DisplayName ?? "User Deleted",
                    CreatedByAvatar = user?.AvatarUrl ?? null,
                    PostCount = discussionPosts.Count,
                    SubscriptionCount = discussionSubscriptions
                });
            }

            return discussions;
        }


        public async Task<DiscussionDto> GetDiscussion(string discussionId)
        {
            var discussion = await _discussionRepository.GetDiscussion(discussionId);
            DiscussionDto discussionDto = _mapper.Map<DiscussionDto>(discussion);

            return discussionDto;
        }


        public async Task<Discussion> CreateDiscussion(CreateDiscussionDto newDiscussion)
        {
            var discussion = new Discussion
            {
                Id = Guid.NewGuid().ToString(),
                Title = newDiscussion.Title,
                UserId = newDiscussion.UserId,
                Description = newDiscussion.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Genres = new List<Genre>()
            };
            // Load the genres from the database or create new genres if they don't exist
            foreach (var genreName in newDiscussion.Genres)
            {
                var genre = await _discussionRepository.GetOrCreateGenre(genreName);
                discussion.Genres.Add(genre);
            }

            var discussionCreated = await _discussionRepository.CreateDiscussion(discussion);
            return discussionCreated;
        }
        public async Task UpdateDiscussionDate(string discussionId)
        {
            var discussion = await _discussionRepository.GetDiscussion(discussionId);

            await _discussionRepository.UpdateDiscussionDate(discussion);
        }
        public async Task<bool> DeleteDiscussion(string discussionId)
        {
            var discussionToDelete = await _discussionRepository.GetDiscussion(discussionId);

            var isDeleted = await _discussionRepository.DeleteDiscussion(discussionToDelete);

            return isDeleted;
        }
        public async Task<int> GetDiscussionCountForUser(string userId)
        {
            try
            {
                var discussionCount = await _discussionRepository.GetDiscussionCountForUserAsync(userId);

                return discussionCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting post count for user {userId}");
                throw;
            }
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await _discussionRepository.GetAllGenres();
        }
        public async Task<bool> ToggleSubscription(string discussionId, string userId)
        {
           var subscribeToDiscussion = await _discussionRepository.ToggleSubscription(discussionId, userId);
            return true;
        }

        public async Task<bool> IsSubscribedToDiscussion(string discussionId, string userId)
        {
            var isSubscribed = await _discussionRepository.GetUserSubscription(discussionId, userId);
            return isSubscribed;
        }
    }
}
