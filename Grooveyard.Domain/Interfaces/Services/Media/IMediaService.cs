using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.DTO.User;

namespace Grooveyard.Domain.Interfaces.Services.Media
{
    public interface IMediaService
    {

        Task<List<TrackDto>> GetUserMusicboxAsync(string userId);
    }
}
