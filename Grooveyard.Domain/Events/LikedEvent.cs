using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Domain.Events
{
    public class LikedEvent : DomainEvent
    {
        public string EntityId { get; }
        public string LikedByUserId { get; }
        public string AuthorId { get; }
        public string TargetType { get; set; }
        public string ParentId { get; set; }


        public LikedEvent(string commentId, string likedByUserId, string authorId, string targetType, string parentId)
        {
            EntityId = commentId;
            LikedByUserId = likedByUserId;
            AuthorId = authorId;
            TargetType = targetType;
            ParentId = parentId;
 
        }
    }
}
