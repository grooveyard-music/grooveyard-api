namespace Grooveyard.Domain.Models.Media
{
    public class SongGenre
    {
        public string SongId { get; set; }
        public Song Song { get; set; }

        public string GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
