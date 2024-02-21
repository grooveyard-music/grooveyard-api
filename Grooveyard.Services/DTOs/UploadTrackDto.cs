using Grooveyard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using static Grooveyard.Domain.Entities.Mix;

namespace Grooveyard.Services.DTOs
{
    public class UploadTrackDto
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public long DurationInMilliseconds { get; set; }
        public TimeSpan Duration => TimeSpan.FromMilliseconds(DurationInMilliseconds);
        public string Type { get; set; }
        public string Uri { get; set; }
        public List<string> Genres { get; set; }
        public HostType Host { get; set; }

    }
}
