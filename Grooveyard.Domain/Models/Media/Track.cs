namespace Grooveyard.Domain.Models.Media
{
    public class Track
    {
        public string Id { get; set; }
        public string Type { get; set; } // Song or Mix
        public string MediaId { get; set; } // Id of either Song or Mix
        public Song Song { get; set; } // Navigation property
        public Mix Mix { get; set; } // Navigation property
        public ICollection<MusicboxTrack> MusicboxTracks { get; set; } = new HashSet<MusicboxTrack>();

    }
}
