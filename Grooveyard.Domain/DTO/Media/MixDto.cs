using static Grooveyard.Domain.Models.Media.Mix;

namespace Grooveyard.Domain.DTO.Media
{
    public class MixDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string UrlPath { get; set; }
        public HostType Host { get; set; }
        public List<string> Genres { get; set; } = new List<string>();

    }

}
