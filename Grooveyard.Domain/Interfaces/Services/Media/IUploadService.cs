using Grooveyard.Domain.DTO.Media;

namespace Grooveyard.Domain.Interfaces.Services.Media
{
    public interface IUploadService
    {
        Task<SongDto> UploadSong(UploadSongDto song, string userId);
        Task<TracklistDto> UploadTracklist(TracklistDto tracklist);
        Task<MixDto> UploadMix(UploadMixDto mix, string userId);
        Task<SongDto> AddSongToTracklist(string tracklistId, string songId);

    }
}
