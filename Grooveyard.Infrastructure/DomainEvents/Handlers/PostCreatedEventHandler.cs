using Grooveyard.Domain.Events;
using Grooveyard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.DomainEvents.Handlers
{
    public class PostCreatedEventHandler : IDomainEventHandler<PostCreatedEvent>
    {
        private readonly INotificationService _notificationService;
        public PostCreatedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(PostCreatedEvent notification)
        {
                await _notificationService.SendSubscriptionNotificationAsync(notification.PostId, notification.DiscussionId);
            
        }
    }
}
