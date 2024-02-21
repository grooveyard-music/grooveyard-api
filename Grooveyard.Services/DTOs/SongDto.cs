using Grooveyard.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Grooveyard.Services.DTOs
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
