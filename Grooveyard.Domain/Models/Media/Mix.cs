namespace Grooveyard.Domain.Models.Media
{
    public class Mix
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public string? UrlPath { get; set; }
        public DateTime CreatedAt { get; set; }

        public HostType Host { get; set; }
        // Navigation properties

        // Relationship to MusicFile
        public string? MusicFileId { get; set; }
        public MusicFile? MusicFile { get; set; }

        // Relationship to Tracklist (one Mix can have one Tracklist)
        public string? TracklistId { get; set; }
        public Tracklist? Tracklist { get; set; }

        public string UserId { get; set; }

        public ICollection<Genre> Genres { get; set; }
        public enum HostType
        {
            SelfHosted,
            YouTube,
            SoundCloud,

        }
        public Mix()
        {
            // other initializations
            Genres = new HashSet<Genre>();
        }



    }
}
