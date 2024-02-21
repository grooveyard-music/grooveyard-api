using Grooveyard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.Services
{
    public interface INotificationService
    {
        Task SendLikeNotificationAsync(string userId, string likedByUserId, string targetId, string targetType, string parentId = null);
        Task SendSubscriptionNotificationAsync(string postId, string discussionId);
        Task<List<Notification>> GetNotificationsAsync(string userId);
        Task<bool> MarkNotificationAsRead(List<string> notificationId);
    }
}
