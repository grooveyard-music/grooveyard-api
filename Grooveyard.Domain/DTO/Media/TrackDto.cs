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
        public string Type { get; set; } 
        public DateTime DateCreated { get; set; }
        public List<SongDto> Songs { get; set; }
        public List<MixDto> Mixes { get; set; }

    }
}
