using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Domain.Entities
{
    public class TracklistTrack
    {
        public string TracklistId { get; set; }
        public string TrackId { get; set; }

        public virtual Tracklist Tracklist { get; set; }
        public virtual Track Track { get; set; }
    }
}
