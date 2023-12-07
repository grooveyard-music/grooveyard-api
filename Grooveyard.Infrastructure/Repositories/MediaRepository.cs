
using Grooveyard.Domain.Interfaces.Repositories.Media;
using Grooveyard.Domain.Models.Media;
using Grooveyard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Grooveyard.Infrastructure.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly GrooveyardDbContext _context;
        private readonly ILogger<MediaRepository> _logger;

        public MediaRepository(GrooveyardDbContext context, ILogger<MediaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Song>> GetUserSongsAsync(string userId)
        {
            var songs = await _context.Songs
             .Where(x => x.UserId == userId)
             .OrderBy(x => x.CreatedAt)
             .Include(x => x.Genres)
             .ToListAsync();

            return songs;
        }

        public async Task<List<Mix>> GetUserMixesAsync(string userId)
        {
            var mixes = await _context.Mixes
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.CreatedAt)
                .Include(x => x.Genres)
                .ToListAsync();

            return mixes;
        }





    }
}

