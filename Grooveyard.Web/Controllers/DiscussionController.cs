using Grooveyard.Services.DTOs;
using Grooveyard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Grooveyard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscussionController : ControllerBase
    {
        private readonly ILogger<DiscussionController> _logger;
        private readonly IDiscussionService _discussionService;

        public DiscussionController(ILogger<DiscussionController> logger, IDiscussionService discussionService)
        {
            _logger = logger;
            _discussionService = discussionService;

        }

        [HttpGet("GetLatestDiscussions")]
        public async Task<IActionResult> GetLatestDiscussions()
        {
            try
            {
                var discussions = await _discussionService.GetDiscussions();

                return Ok(discussions);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting latest discussions");
                var errorResponse = new { Message = "Error occurred while getting latest discussions" };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }


        [HttpPost("CreateDiscussion")]
        public async Task<IActionResult> CreateDiscussion(CreateDiscussionDto newDiscussion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state");
                }
                var result = await _discussionService.CreateDiscussion(newDiscussion);
                return Ok(result.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating discussion");

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new discussion");
            }
        }

        [HttpGet("GetDiscussion/{discussionId}")]
        public async Task<IActionResult> GetDiscussion(string discussionId)
        {
            try
            { 
                var discussion = await _discussionService.GetDiscussion(discussionId);
                return Ok(discussion);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating discussion");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new discussion");
            }
        }

        [HttpDelete("DeleteDiscussion/{discussionId}")]
        public async Task<IActionResult> DeleteDiscussion(string discussionId)
        {
            try
            {
                var result = await _discussionService.DeleteDiscussion(discussionId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while deleting discussion");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost("SubscribeToDiscussion/{discussionId}")]
        public async Task<IActionResult> SubscribeToDiscussion(string discussionId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _discussionService.ToggleSubscription(discussionId, userId);
                if (result)
                {
                    return Ok("Subscribed successfully");
                }
                else
                {
                    return BadRequest("Subscribe failed");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while subscribing to discussion");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new subscription");
            }
        }

        [HttpGet("GetAllGenres")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _discussionService.GetAllGenres();

            return Ok(genres);
        }


        [HttpGet("IsSubscribed/{discussionId}")]
        public async Task<IActionResult> IsSubscribedToDiscussion(string discussionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            var isSubscribed = await _discussionService.IsSubscribedToDiscussion(discussionId, userId);
            return Ok(isSubscribed);
        }
    }



}
