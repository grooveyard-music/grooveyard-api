using Grooveyard.Domain.Events;
using Grooveyard.Domain.Models.Grooveyard.Domain.Models;

namespace Grooveyard.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public PostType Type { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Keys
        public string? UserId { get; set; }
        public string DiscussionId { get; set; }

        // Optional Foreign Keys for Song or Mix
        public string? TrackId { get; set; }

        // Navigation Properties
        public Discussion Discussion { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; } = new List<Like>();


        public void Like(string likedByUserId)
        {
            var postLikedEvent = new LikedEvent(Id, likedByUserId, UserId, "post", DiscussionId);
            this.AddDomainEvent(postLikedEvent);
        }

        public void Suscribed()
        {
            var postLikedEvent = new PostCreatedEvent(DiscussionId, Id);
            this.AddDomainEvent(postLikedEvent);
        }
    }
    public enum PostType
    {
        Track,
        Text
    }
}
