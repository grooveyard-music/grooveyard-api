using Grooveyard.Domain.Models.Media;

namespace Grooveyard.Domain.Interfaces.Repositories.Media
{
    public interface IUploadRepository
    {
        Task<Tracklist> CreateTracklistAsync(Tracklist tracklist);
        Task<Track> UploadTrackAsync(Track track);

        Task<Mix> CreateMixAsync(Mix mix);

        Task<Song> CreateSongAsync(Song song);
        Task<MusicFile> UploadMusicFileAsync(MusicFile file);
        Task<Mix?> GetMixByVideoIdAsync(string videoId);

        Task<List<Genre>> GetGenresByNamesAsync(List<string> genres);
    }
}
