using Grooveyard.Domain.Models.Media;
using Microsoft.AspNetCore.Http;
using static Grooveyard.Domain.Models.Media.Mix;

namespace Grooveyard.Domain.DTO.Media
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
