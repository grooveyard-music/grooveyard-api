﻿using Grooveyard.Domain.Models.Social;

namespace Grooveyard.Domain.DTO.Social
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string DiscussionId { get; set; }
        public string UserId { get; set; }

        public PostType Type { get; set; }


        public string? TrackId { get; set; }

    }

}
