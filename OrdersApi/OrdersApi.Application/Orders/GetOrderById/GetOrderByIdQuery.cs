using MediatR;
using OrdersApi.Application.Common.Dtos;

namespace OrdersApi.Application.Orders.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderDetailsDto>
    {
        public Guid Id { get; init; }
    }
}
