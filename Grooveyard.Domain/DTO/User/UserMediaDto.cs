﻿using Grooveyard.Domain.DTO.Media;

namespace Grooveyard.Domain.DTO.User
{
    public class UserMediaDto
    {
        public List<TracklistDto> Tracklists { get; set; }
        public List<TrackDto> Tracks { get; set; }

    }

}
