using Grooveyard.Domain.Models.Media;
using Microsoft.AspNetCore.Http;

namespace Grooveyard.Domain.DTO.Media
{
    public class SongDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Uri { get; set; }
        public HostType Host { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
    }
}
