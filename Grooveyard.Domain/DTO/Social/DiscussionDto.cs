using Grooveyard.Domain.DTO.User;

namespace Grooveyard.Domain.DTO.Social
{
    public class DiscussionDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Genres { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByUsername { get; set; }
        public string CreatedByAvatar { get; set; }
    }
}
