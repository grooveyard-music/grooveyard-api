using Grooveyard.Domain.Models.Media;
using System.Runtime.CompilerServices;

namespace Grooveyard.Domain.Interfaces.Repositories.Media
{
    public interface IMediaRepository
    {
        Task<List<Song>> GetUserSongsAsync(string userId);
        Task<List<Mix>> GetUserMixesAsync(string userId);

    }
}
