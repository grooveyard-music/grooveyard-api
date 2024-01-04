

using Grooveyard.Domain.DTO.Social;
using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Repositories.Social;
using Grooveyard.Domain.Interfaces.Repositories.User;
using Grooveyard.Domain.Interfaces.Services.Social;
using Grooveyard.Domain.Interfaces.Services.User;
using Grooveyard.Domain.Models.Media;
using Grooveyard.Domain.Models.Social;
using Grooveyard.Domain.Models.User;
using Microsoft.Extensions.Logging;

namespace Grooveyard.Services.SocialSercice
{
    public class DiscussionService : IDiscussionService
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DiscussionService> _logger;

        public DiscussionService(IDiscussionRepository discussionRepository, IUserRepository userRepository, ILogger<DiscussionService> logger)
        {
            _discussionRepository = discussionRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<DiscussionDto>> GetDiscussions()
        {
            var activeDiscussions = await _discussionRepository.GetDiscussions();

            var userIds = activeDiscussions.Select(d => d.UserId).Distinct().ToList();

            // Ensure we have a non-null list of users
            var users = (await _userRepository.GetUserProfilesByIds(userIds));

            var discussionDto = activeDiscussions.Select(d =>
            {
                var user = users.FirstOrDefault(u => u.UserId == d.UserId);

                return new DiscussionDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    CreatedById = user?.UserId ?? null,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Genres = d.Genres.Select(g => g.Name).ToList(),
                    CreatedByUsername = user?.DisplayName ?? "User Deleted",
                    CreatedByAvatar = user?.AvatarUrl ?? null
                };
            }).ToList();

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
        public async Task<bool> SubscribeToDiscussion(string discussionId, string userId)
        {
            return true;
        }
    }
}
