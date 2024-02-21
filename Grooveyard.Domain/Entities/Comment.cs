using Grooveyard.Domain.Events;
using Grooveyard.Domain.Models.Grooveyard.Domain.Models;

namespace Grooveyard.Domain.Entities
{
    public class Comment : BaseEntity
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

        public void Like(string likedByUserId)
        {
            var commentLikedEvent = new LikedEvent(Id, likedByUserId, UserId, "comment", PostId);
            this.AddDomainEvent(commentLikedEvent);
        }
    }

}
