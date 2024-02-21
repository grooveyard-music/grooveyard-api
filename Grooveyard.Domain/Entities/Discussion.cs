using Grooveyard.Domain.Events;
using Grooveyard.Domain.Models.Grooveyard.Domain.Models;

namespace Grooveyard.Domain.Entities
{
    public class Discussion : BaseEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public List<Subscription> Subscriptions { get; set; }

  
    }
}
