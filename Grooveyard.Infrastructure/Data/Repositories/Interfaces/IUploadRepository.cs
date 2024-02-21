using Grooveyard.Domain.Entities;

namespace Grooveyard.Infrastructure.Data.Repositories.Interfaces
{
    public interface IUploadRepository
    {
        Task<Tracklist> CreateTracklistAsync(Tracklist tracklist);
        Task<Track> UploadTrackAsync(Track track);

        Task<Mix> CreateMixAsync(Mix mix);
        Task<Song> CreateSongAsync(Song song);
        Task<Mix?> GetMixByVideoIdAsync(string videoId);
        Task<List<Genre>> GetGenresByNamesAsync(List<string> genres);
    }
}
