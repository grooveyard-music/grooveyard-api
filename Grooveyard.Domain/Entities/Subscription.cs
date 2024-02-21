namespace Grooveyard.Domain.Entities
{
    public class Subscription
    {
        public string UserId { get; set; }

        public string DiscussionId { get; set; }
        public Discussion Discussion { get; set; }
    }

}
