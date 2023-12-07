
using Grooveyard.Domain.Interfaces.Repositories.Social;
using Grooveyard.Domain.Models.Media;
using Grooveyard.Domain.Models.Social;
using Grooveyard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Grooveyard.Infrastructure.Repositories
{
    public class DiscussionRepository : IDiscussionRepository
    {

        private readonly GrooveyardDbContext _context;

        public DiscussionRepository(GrooveyardDbContext context)
        {
            _context = context;

        }
        public async Task<Discussion> CreateDiscussion(Discussion discussion)
        {
            var discussionAdded = _context.Discussions.Add(discussion);
            await _context.SaveChangesAsync();

            return discussionAdded.Entity;
        }

        public async Task<List<Discussion>> GetDiscussions()
        {

            return await _context.Discussions
                .Include(d => d.Genres)
                .OrderByDescending(d => d.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Discussion> GetDiscussion(string discussionId)
        {

            return await _context.Discussions.FirstOrDefaultAsync(x => x.Id == discussionId);
        }

        public async Task<bool> DeleteDiscussion(Discussion discussion)
        {
            _context.Discussions.Remove(discussion);

            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<bool> UpdateDiscussionDate(Discussion discussion)
        {
            discussion.UpdatedAt = DateTime.Now;
            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<Genre> GetOrCreateGenre(string genreName)
        {
            Genre genre = null;
            try
            {
                genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre == null)
                {
                    genre = new Genre { Name = genreName, Id = Guid.NewGuid().ToString() };
                    _context.Genres.Add(genre);
                    _context.SaveChangesAsync().Wait();
                    genre = _context.Genres.FirstOrDefault(g => g.Name == genreName);
                }
            }

            catch (Exception ex)
            {
                // Log the exception or handle it
            }

            return genre;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<int> GetDiscussionCountForUserAsync(string userId)
        {
            return await _context.Discussions.CountAsync(p => p.UserId == userId);
        }

        public async Task<Discussion> SubscribeToDiscussion(string discussionId, string userId)
        {
            return new Discussion();
        }

    }
}
