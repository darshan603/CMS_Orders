using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Common.Dtos
{
    public class OrderListItemDto
    {
        public Guid Id { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public DateTime OrderDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal TotalAmount { get; init; }
    }
}
