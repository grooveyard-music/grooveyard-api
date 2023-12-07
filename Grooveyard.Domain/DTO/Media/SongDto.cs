using Microsoft.AspNetCore.Http;

namespace Grooveyard.Domain.DTO.Media
{
    public class SongDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string UrlPath { get; set; }  // Optional
        public List<string> Genres { get; set; } = new List<string>();
        public IFormFile? MusicFile { get; set; }  // Optional
    }
}
