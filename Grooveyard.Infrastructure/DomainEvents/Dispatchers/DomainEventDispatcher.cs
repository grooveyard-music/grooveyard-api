using Grooveyard.Domain.Events;
using Grooveyard.Infrastructure.DomainEvents.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.DomainEvents.Dispatchers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch(DomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new Exception($"Handler for {eventType.Name} not found.");
            }

            var method = handlerType.GetMethod("Handle");
            await (Task)method.Invoke(handler, new object[] { domainEvent });
        }
    }
}
