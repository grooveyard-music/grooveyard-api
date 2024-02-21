using Grooveyard.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grooveyard.Infrastructure.DomainEvents.Dispatchers
{

    public interface IDomainEventDispatcher
        {
            Task Dispatch(DomainEvent domainEvent);
        }
  }


