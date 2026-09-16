using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Pagination;

public  class PaginatedResult<IEntity>
    (int pageIndex, int pageSize, int count, IEnumerable<IEntity> data)
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public int Count { get; } = count;
    public IEnumerable<IEntity> Data { get; } = data;
}
