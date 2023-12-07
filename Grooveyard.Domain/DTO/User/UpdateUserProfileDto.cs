using Microsoft.AspNetCore.Http;


namespace Grooveyard.Domain.DTO.User
{
    public class UpdateUserProfileDto
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Location { get; set; }
        public string? Biography { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string userId { get; set; }
    }
}
