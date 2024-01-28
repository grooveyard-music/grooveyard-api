using Grooveyard.Domain.Models.Media;
using System.Runtime.CompilerServices;

namespace Grooveyard.Domain.Interfaces.Repositories.Media
{
    public interface IMediaRepository
    {
        Task<List<Song>> GetUserSongsAsync(string userId);
        Task<List<Mix>> GetUserMixesAsync(string userId);
        Task<Musicbox> GetOrCreateUserMusicboxAsync(string userId);
        Task AddTrackToMusicBox(Track track, string musicboxId);
        Task<Track> GetTrackByIdAsync(string trackId);
        Task<Mix> GetMixByUri(string uri);
        Task<Song> GetSongByUri(string uri);
        Task<Track> GetTrackBySongId(string songId);
        Task<Track> GetTrackByMixId(string songId);
    }
}
