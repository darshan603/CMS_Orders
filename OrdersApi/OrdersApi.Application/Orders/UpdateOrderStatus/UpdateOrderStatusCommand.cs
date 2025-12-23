using MediatR;
using OrdersApi.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<OrderStatusUpdatedDto>
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
