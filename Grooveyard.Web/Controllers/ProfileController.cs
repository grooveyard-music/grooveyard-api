using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
        private readonly IUserProfileService _profileService;
        private readonly IAccountService _accountService;
        public ProfileController(IUserProfileService profileService, IAccountService accountService)
        {
            _profileService = profileService;
            _accountService = accountService;
        }

        [HttpGet("getprofile/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
                var currentProfile = await _profileService.GetUserProfile(userId);
                return Ok(currentProfile);
           
        }

        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto userProfile)
        {

            if (userProfile.userId != null) {

             var updatedProfile =  await  _profileService.UpdateUserProfile(userProfile);

             return Ok(updatedProfile);

            }

            return Unauthorized();
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

