using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Domain.DTO.Media
{
    public class TrackDto
    {
        public string Id { get; set; }
        public string Type { get; set; } // "Song" or "Mix"
        public DateTime DateCreated { get; set; }

        // Song and Mix are nullable to accommodate tracks that might not have them set yet
        public SongDto? Song { get; set; }
        public MixDto? Mix { get; set; }

        // Include any additional properties that a Track should have

        // Constructor or method to handle initialization based on Type, if necessary
    }
}
