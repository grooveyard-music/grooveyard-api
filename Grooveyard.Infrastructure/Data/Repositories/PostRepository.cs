using Microsoft.EntityFrameworkCore;
using Grooveyard.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Grooveyard.Domain.Entities;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Grooveyard.Domain.Models.Grooveyard.Domain.Models;
using Grooveyard.Infrastructure.DomainEvents.Dispatchers;

namespace Grooveyard.Infrastructure.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly GrooveyardDbContext _context;
        private readonly ILogger<PostRepository> _logger;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        public PostRepository(GrooveyardDbContext context, ILogger<PostRepository> logger, IDomainEventDispatcher domainEventDispatcher)
        {
            _context = context;
            _logger = logger;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<List<Post>> GetPostsAsync(string discussionId)
        {
            try
            {
                return await _context.Posts
                            .Where(x => x.DiscussionId == discussionId)
                           .Include(x => x.Likes)
                           .Include(x => x.Comments)
                           .ThenInclude(c => c.Likes)
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
                throw new Exception("Error occurred while fetching posts.", ex);
            }
        }

        public async Task<Comment> GetCommentAsync(string commentId)
        {
            try
            {
                return await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching posts.", ex);
            }
        }


        public async Task<Post> GetPostAndCommentsAsync(string postId)
        {
            try
            {
                return await _context.Posts
                                     .Include(x => x.Comments) 
                                     .ThenInclude(c => c.Likes) 
                                     .Include(x => x.Likes) 
                                     .FirstOrDefaultAsync(x => x.Id == postId);
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
                if (postAdded != null)
                {
                    postAdded.Entity.Suscribed();
                    await DispatchDomainEventsAsync(_context);
                }
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


        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            try
            {
                _context.Comments.Remove(comment);
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
                if (!string.IsNullOrEmpty(like.PostId))
                {
                    existingLike = _context.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.PostId == like.PostId);
                }
                if (!string.IsNullOrEmpty(like.CommentId))
                {
                    existingLike = _context.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.CommentId == like.CommentId);
                }

                if (existingLike != null)
                {
                    _context.Likes.Remove(existingLike);
                    await _context.SaveChangesAsync();
                    return null; 
                }
                else
                {
                    var entry = await _context.Likes.AddAsync(like);
                    await _context.SaveChangesAsync();

                    var comment = await _context.Comments.FindAsync(like.CommentId);
                    var post = await _context.Posts.FindAsync(like.PostId);
                    if (comment != null)
                    {
                        comment.Like(like.UserId);
                    }
                    if (post != null)
                    {
                        post.Like(like.UserId);
                    }

                    await DispatchDomainEventsAsync(_context);
                    return entry.Entity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while liking the comment.", ex);
            }
        }

            private async Task DispatchDomainEventsAsync(DbContext context)
            {
                var domainEntities = context.ChangeTracker
                    .Entries<BaseEntity>()
                    .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

                var domainEvents = domainEntities
                    .SelectMany(x => x.Entity.DomainEvents)
                    .ToList();

                domainEntities.ToList()
                    .ForEach(entity => entity.Entity.ClearDomainEvents());

                foreach (var domainEvent in domainEvents)
                {
                    await _domainEventDispatcher.Dispatch(domainEvent);
                }
            }


        
    }
}
