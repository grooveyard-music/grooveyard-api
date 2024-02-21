namespace Grooveyard.Domain.Entities
{
    public class Musicbox
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public ICollection<MusicboxTrack> MusicboxTracks { get; set; } = new HashSet<MusicboxTrack>();
    }

    public class MusicboxTrack
    {
        public string MusicboxId { get; set; }
        public Musicbox Musicbox { get; set; }
        public DateTime DateAdded { get; set; }

        public string TrackId { get; set; }
        public Track Track { get; set; }
    }

}
