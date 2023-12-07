
using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.Interfaces.Services.Media;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadService _uploadService;

        public UploadController(ILogger<UploadController> logger, IUploadService uploadService)
        {
            _logger = logger;
            _uploadService = uploadService;
        }


        [HttpPost("Song")]
        public async Task<IActionResult> UploadSong([FromBody] UploadSongDto songDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return BadRequest("No user found");
                }
                var newSong = await _uploadService.UploadSong(songDto, userId);
                return Ok(newSong);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating a new song");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("Mix")]
        public async Task<IActionResult> UploadMix([FromBody] UploadMixDto mixDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return BadRequest("No user found");
                }

                var newSong = await _uploadService.UploadMix(mixDto, userId);
                return Ok(newSong);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating a new song");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost("UploadTracklist")]
        public IActionResult CreateTracklist(TracklistDto tracklistDto)
        {
            var tracklist = _uploadService.UploadTracklist(tracklistDto);
            return Created($"api/tracklist/{tracklist.Id}", tracklist);
        }

        [HttpPost("{tracklistId}/songs/{songId}")]
        public IActionResult AddSongToTracklist(string tracklistId, string songId)
        {
            _uploadService.AddSongToTracklist(tracklistId, songId);
            return Ok();
        }



    }


}
