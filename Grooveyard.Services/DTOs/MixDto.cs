using Grooveyard.Domain.Entities;
using static Grooveyard.Domain.Entities.Mix;

namespace Grooveyard.Services.DTOs
{
    public class MixDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Uri { get; set; }
        public HostType Host { get; set; }
        public List<string> Genres { get; set; } = new List<string>();

    }

}
