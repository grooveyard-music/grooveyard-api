﻿using Grooveyard.Domain.Models.Media;
using Microsoft.AspNetCore.Http;
using static Grooveyard.Domain.Models.Media.Mix;

namespace Grooveyard.Domain.DTO.Media
{
    public class UploadTrackDto
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public string Type { get; set; }
        public string UrlPath { get; set; }
        public List<string> Genres { get; set; }
        public IFormFile? MusicFile { get; set; }
        public HostType Host { get; set; }

    }
}
