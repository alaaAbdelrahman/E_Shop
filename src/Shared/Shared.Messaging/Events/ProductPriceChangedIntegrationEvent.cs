using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Events;

public class ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }

    public List<string> Category { get; set; } = new List<string>();

    public string Description { get; set; }

    public string ImageUrl { get; set; }


    public decimal Price { get; set; }
}
