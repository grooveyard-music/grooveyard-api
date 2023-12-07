using Grooveyard.Domain.Models.Social;

namespace Grooveyard.Domain.Models.Media
{
    public class Genre
    {
        public string Id { get; set; }
        public string Name { get; set; }

        // Navigation Property
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<Mix> Mixes { get; set; }
        public ICollection<Song> Songs { get; set; }


        public Genre()
        {
            Discussions = new HashSet<Discussion>();
            Mixes = new HashSet<Mix>();
            Songs = new HashSet<Song>();

        }
    }
}
