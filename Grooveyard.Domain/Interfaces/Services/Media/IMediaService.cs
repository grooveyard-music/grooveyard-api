using Grooveyard.Domain.DTO.User;

namespace Grooveyard.Domain.Interfaces.Services.Media
{
    public interface IMediaService
    {

        Task<UserMediaDto> GetUserMediaAsync(string userId);
    }
}
