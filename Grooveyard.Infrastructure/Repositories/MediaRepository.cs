
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

        public async Task<Musicbox> GetOrCreateUserMusicboxAsync(string userId)
        {
            var musicbox = await _context.MusicBoxes
                .Include(x => x.MusicboxTracks)
                .FirstOrDefaultAsync(mb => mb.UserId == userId);

            if (musicbox == null)
            {
                musicbox = new Musicbox { Id = Guid.NewGuid().ToString(), UserId = userId };
                await _context.MusicBoxes.AddAsync(musicbox);
                await _context.SaveChangesAsync();
            }

            return musicbox;
        }

        public async Task AddTrackToMusicBox(Track track, string musicboxId)
        {
            var musicboxTrack = new MusicboxTrack
            {
                MusicboxId = musicboxId,
                TrackId = track.Id,
                DateAdded = DateTime.Now,
            };

            _context.MusicboxTracks.Add(musicboxTrack);
            await _context.SaveChangesAsync();
        }

        public async Task<Track> GetTrackByIdAsync(string trackId)
        {
            var track = await _context.Tracks
                .Where(x => x.Id == trackId)
                .Include(x => x.Songs).ThenInclude(s => s.Genres)
                .Include(x => x.Mixes).ThenInclude(s => s.Genres)
                .FirstOrDefaultAsync();

            return track;
        }

        public async Task<Song> GetSongByUri(string uri)
        {
            // Assuming 'UrlPath' is a property of the Song entity
            return await _context.Songs
                                 .FirstOrDefaultAsync(s => s.Uri == uri);
        }

        public async Task<Mix> GetMixByUri(string uri)
        { 
            return await _context.Mixes
                                 .FirstOrDefaultAsync(s => s.Uri == uri);
        }

        public async Task<Track> GetTrackBySongId(string songId)
        {
            var track = await _context.Songs
                .Where(s => s.Id == songId)
                .Select(s => s.Track)
                .Include(t => t.Songs)
                .FirstOrDefaultAsync();

            return track;
        }


        public async Task<Track> GetTrackByMixId(string mixId)
        {
            var track = await _context.Mixes
                      .Where(s => s.Id == mixId)
                      .Select(s => s.Track)
                      .Include(t => t.Songs)
                      .FirstOrDefaultAsync();

            return track;
        }
    }
}

