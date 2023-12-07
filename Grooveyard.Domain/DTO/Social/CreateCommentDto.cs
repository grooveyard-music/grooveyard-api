namespace Grooveyard.Domain.DTO.Social
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public string PostId { get; set; }
        public string? UserId { get; set; }
    }
}
