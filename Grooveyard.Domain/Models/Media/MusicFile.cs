namespace Grooveyard.Domain.Models.Media
{
    public class MusicFile
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;

        // Optional reference to a Song
        public string? SongId { get; set; }
        public Song? Song { get; set; }

        public Mix? Mix { get; set; }
    }
}