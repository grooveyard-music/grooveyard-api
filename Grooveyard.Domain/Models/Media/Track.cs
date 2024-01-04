namespace Grooveyard.Domain.Models.Media
{
    public class Track
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }

        public string? SongId { get; set; }
        public string? MixId { get; set; }

        public virtual Song Song { get; set; }
        public virtual Mix Mix { get; set; }
        public ICollection<MusicboxTrack> MusicboxTracks { get; set; } = new HashSet<MusicboxTrack>();

    }
}
