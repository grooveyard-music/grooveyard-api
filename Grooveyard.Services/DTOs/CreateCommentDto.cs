namespace Grooveyard.Services.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public string PostId { get; set; }
        public string? UserId { get; set; }
    }
}
