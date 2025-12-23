using MediatR;
using OrdersApi.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderDetailsDto>
    {
        // Customer name for the order
        public string CustomerName { get; init; } = string.Empty;

        // Lines included in the order
        public List<CreateOrderLineDto> Lines { get; init; } = new();
    }
}
