using Grooveyard.Domain.Interfaces.Services.Social;
using Microsoft.AspNetCore.Mvc;

namespace Grooveyard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly ILogger<CommunityController> _logger;
        private readonly ICommunityService _communityService;

        public CommunityController(ILogger<CommunityController> logger, ICommunityService communityService)
        {
            _logger = logger;
            _communityService = communityService;
        }


        [HttpGet("GetUserCommunity")]
        public async Task<IActionResult> GetUserCommunity(string userId)
        {
            try
            {
                var discussions = await _communityService.GetUserCommunityActivity(userId);
                return Ok(discussions);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting latest discussions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

 
    }



}
