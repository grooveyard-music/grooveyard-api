using Grooveyard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Services.DTOs
{
    public class LikeDto
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }

        public string? PostId { get; set; }
        public string? CommentId { get; set; }

    }
}
