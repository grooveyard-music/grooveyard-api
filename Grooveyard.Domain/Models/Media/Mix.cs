namespace Grooveyard.Domain.Models.Media
{
    public class Mix
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public string Uri { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public HostType Host { get; set; }
        public string UserId { get; set; }
        public string TrackId { get; set; }
        public virtual Track Track { get; set; }
        public ICollection<Genre> Genres { get; set; }


        public string TracklistId { get; set; }
        public virtual Tracklist Tracklist { get; set; }



    }
}
