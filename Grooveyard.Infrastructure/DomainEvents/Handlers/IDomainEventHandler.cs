using Grooveyard.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.DomainEvents.Handlers
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        Task Handle(TDomainEvent domainEvent);
    }
}
