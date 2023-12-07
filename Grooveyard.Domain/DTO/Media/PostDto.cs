using Grooveyard.Domain.DTO.Social;

namespace Grooveyard.Domain.DTO.Media
{
    public class PostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Genres { get; set; }
        public string CreatedById { get; set; }
        public List<CommentDto> Comments { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
    }
}
