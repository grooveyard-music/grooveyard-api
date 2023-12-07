namespace Grooveyard.Domain.Models.Media
{
    public class Tracklist
    {
        public string Id { get; set; }
        public string Name { get; set; }

        // Status: Draft or Live
        public TracklistStatus Status { get; set; }

        // FK to User, a tracklist belongs to a user
        public string UserId { get; set; }

        // FK to Mix, a tracklist can belong to a mix
        public string? MixId { get; set; }
        public Mix? Mix { get; set; }

        // Navigation Properties
        public ICollection<TracklistSong> TracklistSongs { get; set; }

        public Tracklist()
        {
            TracklistSongs = new HashSet<TracklistSong>();
        }
    }

    public enum TracklistStatus
    {
        Draft,
        Live
    }
}