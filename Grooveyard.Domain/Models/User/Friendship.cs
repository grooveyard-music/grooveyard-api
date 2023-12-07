namespace Grooveyard.Domain.Models.User
{
    public class Friendship
    {
        public string UserId { get; set; }  // FK for user who initiated the friendship
        public UserProfile User { get; set; } // Navigation property

        public string FriendId { get; set; }  // FK for user who received the friendship request
        public UserProfile Friend { get; set; }

        public FriendshipStatus Status { get; set; }  // Status of the friendship

        public DateTime DateInitiated { get; set; }  // Date when the friendship was initiated or request was sent
        public DateTime? DateAccepted { get; set; }  // Date when the friendship was accepted

        // Additional fields as required, e.g., DateBlocked, etc.
    }

    public enum FriendshipStatus
    {
        Pending,   // Friend request sent but not yet accepted
        Accepted,  // Friend request accepted
        Declined,  // Friend request declined
        Blocked    // One user has blocked the other
    }

}
