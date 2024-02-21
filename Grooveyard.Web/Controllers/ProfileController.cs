using Grooveyard.Services.DTOs;
using Grooveyard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
        private readonly IUserProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(IUserProfileService profileService,  ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        [HttpGet("getprofile/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
                var currentProfile = await _profileService.GetUserProfile(userId);
                return Ok(currentProfile);
           
        }

        [HttpGet("musicbox/{userId}")]
        public async Task<IActionResult> GetUserMusicbox(string userId)
        {
            try
            {
                var musicbox = await _profileService.GetUserMusicboxAsync(userId);
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

        [HttpGet("musicbox/search/{userId}")]
        public async Task<IActionResult> SearchMusicbox(string userId, [FromQuery] string searchTerm)
        {
            try
            {
                var musicbox = await _profileService.SearchUserMusicboxAsync(userId, searchTerm);
                return Ok(musicbox);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting user mixes");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileDto userProfile)
        {

            if (userProfile.userId != null) {

             var updatedProfile =  await  _profileService.UpdateUserProfile(userProfile);

             return Ok(updatedProfile);

            }

            return Unauthorized();
        }

        [HttpPut("updateavatar/{userId}")]
        public async Task<IActionResult> UpdateUserProfileAvatar(string userId, [FromForm] IFormFile imageFile)
        {

            var user = User;
            if (user.FindFirst(ClaimTypes.NameIdentifier)?.Value != userId)
            {
                return Unauthorized();
            }
            if (userId != null)
            {
                var updatedProfile = await _profileService.UpdateUserProfileAvatar(userId, imageFile);

                return Ok(updatedProfile);

            }

            return BadRequest();
        }

        [HttpPut("updatecover/{userId}")]
        public async Task<IActionResult> UpdateUserProfileCover(string userId, [FromForm] IFormFile imageFile)
        {

            var user = User;
            if (user.FindFirst(ClaimTypes.NameIdentifier)?.Value != userId)
            {
                return Unauthorized();
            }
            if (userId != null)
            {
                var updatedProfile = await _profileService.UpdateUserProfileCover(userId, imageFile);

                return Ok(updatedProfile);

            }

            return BadRequest();
        }

        [HttpPut("createprofile")]
        public async Task<IActionResult> CreateUserProfile(UpdateUserProfileDto userProfile)
        {

            if (userProfile.userId != null)
            {

                var updatedProfile = await _profileService.UpdateUserProfile(userProfile);

                return Ok(updatedProfile);

            }

            return Unauthorized();
        }


        [HttpPost("check-name")]
        public async Task<IActionResult> CheckDisplayName([FromQuery] string displayName)
        {
     
            var nameExists = await _profileService.CheckDisplayName(displayName);

            return Ok(nameExists);
        }

    }
    }

