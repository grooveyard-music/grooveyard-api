using Grooveyard.Domain.Entities;
using Grooveyard.Domain.Events;
using Grooveyard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly GrooveyardDbContext _context;

        public NotificationService(GrooveyardDbContext context)
        {
            _context = context;

        }
        public async Task SendLikeNotificationAsync(string userId, string likedByUserId, string targetId, string targetType, string parentId = null)
        {
           var userToSend = _context.Users.FirstOrDefault(x => x.Id == likedByUserId);

            string message = null;
            if (targetType == "comment")
                message = $"Your comment was liked by {userToSend.UserName}";
            if (targetType == "post")
                message = $"Your post was liked by {userToSend.UserName}";

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Message = message,
                TargetType = targetType,
                TargetId = targetId,
                ParentId = parentId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendSubscriptionNotificationAsync(string postId, string discussionId)
        {
            var subscriptions = await _context.Subscriptions
                               .Where(s => s.DiscussionId == discussionId)
                               .ToListAsync();
            var discussion = await _context.Discussions.FirstOrDefaultAsync(x => x.Id == discussionId);
                               
                          
            foreach (var subscription in subscriptions)
            {

                var message = $"New post has been added to {discussion.Title}";
                var notification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = subscription.UserId,
                    Message = message,
                    TargetType = "discussion",
                    TargetId = postId,
                    ParentId = discussionId
                };

                _context.Notifications.Add(notification);
            }

        
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            var notifications = await _context.Notifications
                                       .Where(n => n.UserId == userId)
                                       .ToListAsync();

            return notifications;
        }

        public async Task<bool> MarkNotificationAsRead(List<string> notificationIds)
        {

            foreach(var notificationId in notificationIds)
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }   
            return false;

        }
    }
}
