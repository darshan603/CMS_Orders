using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Common.Exceptions;
using OrdersApi.Application.Interfaces;
using OrdersApi.Domain.Exceptions;

namespace OrdersApi.Application.Orders.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrdersDbContext _db;
        public DeleteOrderHandler(IOrdersDbContext db)
        {
            _db = db;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            // Load order
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order is null)
                throw new NotFoundException($"Order '{request.Id}' was not found.");

            try
            {
                // Enforce domain delete rule (only Pending)
                order.EnsureCanDelete();
            }
            catch (DomainException ex)
            {
                // Convert to 409 conflict
                throw new BusinessRuleException(ex.Message);
            }

            // Remove order (lines will cascade delete)
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
