using Microsoft.AspNetCore.Http;


namespace Grooveyard.Services.DTOs
{
    public class UpdateUserProfileDto
    {

        public string? FullName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Location { get; set; }
        public string? Biography { get; set; }
        public string userId { get; set; }
    }
}
