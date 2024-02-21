namespace Grooveyard.Services.DTOs
{
    public class TracklistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SongDto> SongDtos { get; set; }

    }
}
