﻿namespace Grooveyard.Services.DTOs
{
    public class CreateDiscussionDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public List<string> Genres { get; set; }
    }
}
