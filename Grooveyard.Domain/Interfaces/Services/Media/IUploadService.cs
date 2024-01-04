using Grooveyard.Domain.DTO.Media;

namespace Grooveyard.Domain.Interfaces.Services.Media
{
    public interface IUploadService
    {
        Task<TrackDto> UploadTrack(UploadTrackDto song, string userId);
        Task<TracklistDto> UploadTracklist(TracklistDto tracklist);
        Task<SongDto> AddSongToTracklist(string tracklistId, string songId);

    }
}
