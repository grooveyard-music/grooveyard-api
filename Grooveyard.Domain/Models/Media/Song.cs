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
        public string? UrlPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Genre> Genres { get; set; }

        public string UserId { get; set; }


        // Navigation Properties
        public ICollection<TracklistSong> TracklistSongs { get; set; }

        // Optional music file
        public string? MusicFileId { get; set; }
        public MusicFile? MusicFile { get; set; }

        public ICollection<MusicboxTrack> MusicboxTracks { get; set; } = new HashSet<MusicboxTrack>();

        public Song()
        {
            TracklistSongs = new HashSet<TracklistSong>();
        }
    }

    public class TracklistSong
    {
        public string TracklistId { get; set; }
        public string SongId { get; set; }

        // Navigation Properties
        public Tracklist Tracklist { get; set; }
        public Song Song { get; set; }
    }

}
