using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Domain.Events
{
    public class PostCreatedEvent : DomainEvent
    {
        public string DiscussionId { get; }
        public string PostId { get; }

        public PostCreatedEvent(string discussionId, string postId)
        {
            DiscussionId = discussionId;
            PostId = postId;
        }
    }
}
