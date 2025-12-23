using MediatR;

namespace OrdersApi.Application.Orders.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public Guid Id { get; init; }
    }
}
