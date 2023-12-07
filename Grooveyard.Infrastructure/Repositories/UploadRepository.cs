﻿
using Grooveyard.Domain.Interfaces.Repositories.Media;
using Grooveyard.Domain.Models.Media;
using Grooveyard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Grooveyard.Infrastructure.Repositories
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

        public async Task<MusicFile> UploadMusicFileAsync(MusicFile file)
        {
            var fileAdded = _context.MusicFiles.Add(file);
            _context.SaveChanges();

            return fileAdded.Entity;
        }

        public async Task<Song> UploadSongAsync(Song song)
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

        public async Task<Mix> UploadMixAsync(Mix mix)
        {

            var mixAdded = _context.Mixes.Add(mix);
            _context.SaveChanges();

            return mixAdded.Entity;
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
                .Where(m => m.UrlPath.Contains(videoId))
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
