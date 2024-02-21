using Grooveyard.Domain.Events;
using Grooveyard.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.DomainEvents.Handlers
{
    public class LikedEventHandler : IDomainEventHandler<LikedEvent>
    {
        private readonly INotificationService _notificationService;
        public LikedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(LikedEvent domainEvent)
        {
       
            await _notificationService.SendLikeNotificationAsync(domainEvent.AuthorId, domainEvent.LikedByUserId, domainEvent.EntityId, domainEvent.TargetType, domainEvent.ParentId);
        }
    }
}
