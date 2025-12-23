using MediatR;
using OrdersApi.Application.Common.Dtos;

namespace OrdersApi.Application.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<OrderStatusUpdatedDto>
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
