namespace Grooveyard.Domain.DTO.Social
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByAvatar { get; set; }
        public int TotalLikes { get; set; }
    }
}
