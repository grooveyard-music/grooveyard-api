using Grooveyard.Services.DTOs;

namespace Grooveyard.Services.Interfaces
{
    public interface IUploadService
    {
        Task<TrackDto> UploadTrack(UploadTrackDto song, string userId);
        Task<TracklistDto> UploadTracklist(TracklistDto tracklist);
        Task<SongDto> AddSongToTracklist(string tracklistId, string songId);

    }
}
