using Grooveyard.Services.DTOs;
using Grooveyard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Grooveyard.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;

        public PostController(ILogger<PostController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpGet("GetPosts/{discussionId}")]
        public async Task<IActionResult> Get(string discussionId) 
        {
            try
            {
                var posts = await _postService.GetPostsAsync(discussionId);
                return Ok(posts);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting latest discussions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetPost/{postId}")]
        public async Task<IActionResult> GetPost(string postId)
        {
            try
            {
                var posts = await _postService.GetPostAndCommentsAsync(postId);
                return Ok(posts);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting latest discussions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            try
            {
                var post = await _postService.CreatePostAsync(createPostDto, createPostDto.UserId);
                return Ok(post);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting latest discussions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

  
        [HttpDelete("DeletePost/{postId}")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            try
            {
                var result = await _postService.DeletePostAsync(postId);

                if (result)
                {
                    return Ok("Delete success");
                }
                else
                {
                    return BadRequest("Delete failure");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating discussion");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new discussion");
            }
        }

        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            try
            {
                var result = await _postService.DeleteCommentAsync(commentId);

                if (result)
                {
                    return Ok("Delete success");
                }
                else
                {
                    return BadRequest("Delete failure");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating discussion");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new discussion");
            }
        }


        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(CreateCommentDto commentDto)
        {
 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("No user found");
            }
           
            commentDto.UserId = userId;
            var comment = await _postService.CreateCommentAsync(commentDto);

            return Ok(comment);
        }

        [HttpGet("GetComments/{postId}")]
        public async Task<IActionResult> GetComments(string postId)
        {
            try
            {
                var posts = await _postService.GetCommentsAsync(postId);
                return Ok(posts);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting latest discussions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


   
        [HttpPost("post/like")]
        public async Task<IActionResult> TogglePostLike(ToggleLikeDto createLikeDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("No user found");
            }

            createLikeDto.UserId = userId;
            var like = await _postService.TogglePostLikeAsync(createLikeDto);
            return Ok(like);
        }

        [HttpPost("comment/like")]
        public async Task<IActionResult> ToggleCommentLike(ToggleLikeDto createLikeDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("No user found");
            }

            createLikeDto.UserId = userId;
            var like = await _postService.ToggleCommentLikeAsync(createLikeDto);

            return Ok(like);
        }

    }
}
