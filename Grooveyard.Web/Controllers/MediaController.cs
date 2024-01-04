using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Services.Media;
using Microsoft.AspNetCore.Mvc;

namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<MediaController> _logger;
        public MediaController(ILogger<MediaController> logger, IMediaService mediaService)
        {
            _mediaService = mediaService;
            _logger = logger;
        }

        [HttpGet("musicbox/{userId}")]
        public async Task<IActionResult> GetUserMusicbox(string userId)
        {
            try
            {
                var musicbox = await _mediaService.GetUserMusicboxAsync(userId);

                var userMedia = new UserMediaDto
                {
                    Tracks = musicbox
                };

                return Ok(userMedia);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting user mixes");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}



