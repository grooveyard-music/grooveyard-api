namespace Grooveyard.Domain.Entities
{
    public class Tracklist
    {
        public string Id { get; set; }
        public TracklistStatus Status { get; set; }
        public string UserId { get; set; }
        public virtual Mix Mix { get; set; }

        public ICollection<TracklistTrack> TracklistTracks { get; set; } = new HashSet<TracklistTrack>();
    }

    public enum TracklistStatus
    {
        Draft,
        Live
    }
}