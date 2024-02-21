using Grooveyard.Domain.Entities;
using Grooveyard.Infrastructure.Data;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Grooveyard.Infrastructure.Data.Repositories
{
    public class UploadRepository : IUploadRepository
    {

        private readonly GrooveyardDbContext _context;
        private readonly ILogger<UploadRepository> _logger;

        public UploadRepository(GrooveyardDbContext context, ILogger<UploadRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Track> UploadTrackAsync(Track track)
        {
            try
            {
                var trackAdded = _context.Tracks.Add(track);
                _context.SaveChanges();
                return trackAdded.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching posts.", ex);
            }

        }

        public async Task<Song> CreateSongAsync(Song song)
        {
            try
            {
                var songAdded = _context.Songs.Add(song);
                _context.SaveChanges();
                return songAdded.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching posts.", ex);
            }

        }

        public async Task<Mix> CreateMixAsync(Mix mix)
        {
            try
            {
                var mixAdded = _context.Mixes.Add(mix);
                _context.SaveChanges();
                return mixAdded.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Error occurred while fetching posts.", ex);
            }

        }

        public async Task<Tracklist> CreateTracklistAsync(Tracklist tracklist)
        {
            var tracklistAdded = _context.Tracklists.Add(tracklist);
            await _context.SaveChangesAsync();

            return tracklistAdded.Entity;
        }


        public async Task<Mix?> GetMixByVideoIdAsync(string videoId)
        {
            return await _context.Mixes
                .Where(m => m.Uri.Contains(videoId))
                .FirstOrDefaultAsync();
        }

        public async Task<List<Genre>> GetGenresByNamesAsync(List<string> genres)
        {

            List<Genre> genresToAdd = new List<Genre>();
            try
            {
                foreach (var genre in genres)
                {
                    var genreToAdd = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genre);
                    if (genreToAdd == null)
                    {
                        var newGenre = new Genre { Name = genre, Id = Guid.NewGuid().ToString() };
                        _context.Genres.Add(newGenre);
                        _context.SaveChangesAsync().Wait();
                        newGenre = _context.Genres.FirstOrDefault(g => g.Name == genre);
                        genresToAdd.Add(newGenre);
                    }
                    else
                    {
                        genresToAdd.Add(genreToAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it
            }

            return genresToAdd;
        }

    }
}
