namespace Grooveyard.Services.DTOs
{
    public class PostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Genres { get; set; }
        public TrackDto? Track { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByUsername { get; set; }
        public string CreatedByAvatar { get; set; }
        public List<CommentDto> Comments { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
    }
}
