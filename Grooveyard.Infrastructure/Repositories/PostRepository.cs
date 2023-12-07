using Microsoft.EntityFrameworkCore;
using Grooveyard.Domain.Interfaces.Repositories.Social;
using Grooveyard.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Grooveyard.Domain.Models.Social;
using Grooveyard.Domain.Models.Media;

namespace Grooveyard.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly GrooveyardDbContext _context;
        private readonly ILogger<PostRepository> _logger;
        public PostRepository(GrooveyardDbContext context, ILogger<PostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Post>> GetPostsAsync(string discussionId)
        {
            try
            {
                return await _context.Posts
                           .Include(x => x.Likes)
                           .Include(x => x.Comments)
                           .Where(x => x.DiscussionId == discussionId)
                           .OrderByDescending(d => d.CreatedAt)
                           .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching posts.", ex);
            }
        }



        public async Task<Post> GetPostAsync(string postId)
        {
            try
            {
                return await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching the post.", ex);
            }
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            try
            {
                var postAdded = _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return postAdded.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while creating the post.", ex);
            }
        }

        public async Task<bool> DeletePostAsync(Post post)
        {
            try
            {
                _context.Posts.Remove(post);
                var saveResult = await _context.SaveChangesAsync();
                return saveResult > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while deleting the post.", ex);
            }
        }


        public async Task<Genre> GetOrCreateGenre(string genreName)
        {
            try
            {
                Genre genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre == null)
                {
                    genre = new Genre { Name = genreName, Id = Guid.NewGuid().ToString() };
                    _context.Genres.Add(genre);
                    await _context.SaveChangesAsync();
                    genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                }
                return genre;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching or creating genre.", ex);
            }
        }

        public async Task<int> GetPostCountForUserAsync(string userId)
        {
            try
            {
                return await _context.Posts.CountAsync(p => p.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while getting post count for user.", ex);
            }
        }

        public async Task<int> GetCommentCountForUserAsync(string userId)
        {
            try
            {
                return await _context.Comments.CountAsync(p => p.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while getting comment count for user.", ex);
            }
        }

        public async Task<List<Comment>> GetCommentsAsync(string postId)
        {
            try
            {
                return await _context.Comments
                       .Where(x => x.PostId == postId)
                       .OrderByDescending(d => d.CreatedAt)
                       .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching posts.", ex);

            }
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            try
            {
                var commentAdded = _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return commentAdded.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while creating the comment.", ex);
            }
        }


        public async Task<Like> ToggleLikeAsync(Like like)
        {
            try
            {
                Like existingLike = null;

                // Check if the like already exists for a post
                if (!string.IsNullOrEmpty(like.PostId))
                {
                    existingLike = _context.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.PostId == like.PostId);
                }

                // Check if the like already exists for a comment
                if (!string.IsNullOrEmpty(like.CommentId))
                {
                    existingLike = _context.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.CommentId == like.CommentId);
                }

                if (existingLike != null)
                {
                    // If the like already exists, remove it
                    _context.Likes.Remove(existingLike);
                    await _context.SaveChangesAsync();
                    return null; // or return the removed like if needed
                }
                else
                {
                    // If the like doesn't exist, add it
                    var entry = await _context.Likes.AddAsync(like);
                    await _context.SaveChangesAsync();
                    return entry.Entity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while liking the comment.", ex);
            }


        }
    }
}
