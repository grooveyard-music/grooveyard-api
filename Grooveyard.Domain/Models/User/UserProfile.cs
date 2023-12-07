using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Grooveyard.Domain.Models.User
{
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Location { get; set; }
        public string? Biography { get; set; }
        public string? AvatarUrl { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        public ICollection<Friendship> InitiatedFriendships { get; set; } = new List<Friendship>();
        public ICollection<Friendship> ReceivedFriendships { get; set; } = new List<Friendship>();

    }

}
