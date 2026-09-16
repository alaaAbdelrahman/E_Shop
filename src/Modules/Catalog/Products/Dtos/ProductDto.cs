using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Dtos;

public record ProductDto
{
    public   Guid Id { get; init; }
    public  string Name { get; init; }
  public   List<string> Category { get; init; }
  public   string Description { get; init; }
  public   string ImageFile { get; init; }
   public  decimal Price { get; init; }
}
