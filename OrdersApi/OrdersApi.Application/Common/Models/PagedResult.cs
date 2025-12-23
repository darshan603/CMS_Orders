using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Common.Models
{
    public class PagedResult<T>
    {
        // Current page number (1-based)
        public int Page { get; init; }

        // Page size used for pagination
        public int PageSize { get; init; }

        // Total items count before pagination
        public int TotalCount { get; init; }

        // Items for the requested page
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    }
}
