using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Events;

public  class IntegrationEvent
{
    public Guid EventId =>Guid.NewGuid();

    public DateTime OccuredOn => DateTime.UtcNow;

    public string EventType => GetType().AssemblyQualifiedName;

}
