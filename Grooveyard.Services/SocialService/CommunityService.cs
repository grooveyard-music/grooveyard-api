using AutoMapper;
using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Services.Social;
using Grooveyard.Infrastructure.Data;

namespace Grooveyard.Services.SocialSercice
{

    public class CommunityService : ICommunityService
    {
        private readonly GrooveyardDbContext _context;
        private readonly IPostService _postService;
        private readonly IDiscussionService _discussionService;
        private readonly IMapper _mapper;
        public CommunityService(GrooveyardDbContext context, IPostService postService, IDiscussionService discussionService, IMapper mapper)
        {
            _context = context;
            _postService = postService;
            _discussionService = discussionService;
            _mapper = mapper;
        }


        public async Task<UserActivityDto> GetUserCommunityActivity(string userId)
        {

            var userActivity = new UserActivityDto
            {
                postCount = await _postService.GetPostCountForUser(userId),
                discussionCount = await _discussionService.GetDiscussionCountForUser(userId),
                commentCount = await _postService.GetCommentCountForUser(userId)
            };
        
            return userActivity;
        }


    }

}
