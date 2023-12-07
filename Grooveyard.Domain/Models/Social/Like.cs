namespace Grooveyard.Domain.Models.Social
{

    public class Like
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }

        public string? PostId { get; set; }
        public Post? Post { get; set; }

        public string? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}



