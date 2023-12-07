using Grooveyard.Domain.Models.Media;


namespace Grooveyard.Domain.Models.Social
{
    public class Discussion
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
