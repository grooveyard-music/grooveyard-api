
using System.ComponentModel.DataAnnotations;

namespace Grooveyard.Services.DTOs
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Location { get; set; }
        public string? Biography { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CoverUrl { get; set; }
        public UserActivityDto UserActivity { get; set; }
    }
}
