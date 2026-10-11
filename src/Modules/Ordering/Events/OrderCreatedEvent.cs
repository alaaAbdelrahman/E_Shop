using Ordering.Models;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Events;

public  class OrderCreatedEvent(Order order) 
    :IDomainEvent
{
}
