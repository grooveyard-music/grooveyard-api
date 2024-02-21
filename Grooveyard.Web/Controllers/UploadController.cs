using Grooveyard.Services.DTOs;
using Grooveyard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Security.Claims;

namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadService _uploadService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UploadController(ILogger<UploadController> logger, IUploadService uploadService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _uploadService = uploadService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        [HttpPost("track")]
        public async Task<IActionResult> UploadTrack([FromBody] UploadTrackDto trackDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return BadRequest("No user found");
                }
                var newSong = await _uploadService.UploadTrack(trackDto, userId);
                return Ok(newSong);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating a new song");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
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

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var clientId = _configuration.GetValue<string>("SoundCloud:ClientId"); // Make sure to store your client ID in appsettings.json or other configuration
            var httpClient = _httpClientFactory.CreateClient();
            var soundCloudSearchUrl = $"https://api-v2.soundcloud.com/search/tracks?q={Uri.EscapeDataString(searchTerm)}&client_id={clientId}&limit=20";

            var response = await httpClient.GetAsync(soundCloudSearchUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content); 
            }

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }



}



