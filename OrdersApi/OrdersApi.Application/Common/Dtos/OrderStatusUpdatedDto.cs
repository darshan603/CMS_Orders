using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Common.Dtos
{
    public class OrderStatusUpdatedDto
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
