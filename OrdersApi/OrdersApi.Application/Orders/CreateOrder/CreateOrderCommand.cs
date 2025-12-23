using MediatR;
using OrdersApi.Application.Common.Dtos;

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
