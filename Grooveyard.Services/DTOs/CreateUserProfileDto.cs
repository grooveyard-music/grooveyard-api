using Microsoft.AspNetCore.Http;

namespace Grooveyard.Services.DTOs
{
    public class CreateUserProfileDto
    {
        public string DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Location { get; set; }
        public string? Biography { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string userId { get; set; }
    }
}
