using static Grooveyard.Domain.Models.Media.Mix;

namespace Grooveyard.Domain.Models.Media
{
    public class Song
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public HostType Host { get; set; }
        public string UrlPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }

        public string TrackId { get; set; }
        public virtual Track Track { get; set; }
        public ICollection<Genre> Genres { get; set; }

        public string MusicFileId { get; set; }
        public virtual MusicFile MusicFile { get; set; }
    }



}
