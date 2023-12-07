
using System.ComponentModel.DataAnnotations;

namespace Grooveyard.Domain.DTO.User
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Location { get; set; }
        public string? Biography { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
