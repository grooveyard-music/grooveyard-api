namespace Grooveyard.Domain.Models.Social
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public string PostId { get; set; }

        // Navigation Properties

        public Post Post { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }

}
