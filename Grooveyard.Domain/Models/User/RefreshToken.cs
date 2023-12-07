
using Microsoft.AspNetCore.Identity;

namespace Grooveyard.Domain.Models.User
{

    public class RefreshToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Token { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public bool Revoked { get; set; } = false;
    }
}


