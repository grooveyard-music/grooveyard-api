namespace Grooveyard.Domain.Models.Media
{
    public class Track
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
        public ICollection<Mix> Mixes { get; set; } = new HashSet<Mix>();
        public ICollection<MusicboxTrack> MusicboxTracks { get; set; } = new HashSet<MusicboxTrack>();

    }

    public enum HostType
    {
        Spotify,
        YouTube,
        SoundCloud,

    }
}
