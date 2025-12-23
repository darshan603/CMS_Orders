using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Common.Exceptions;
using OrdersApi.Application.Interfaces;
using OrdersApi.Domain.Enums;
using OrdersApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, OrderStatusUpdatedDto>
    {
        private readonly IOrdersDbContext _db;
        public UpdateOrderStatusHandler(IOrdersDbContext db)
        {
            _db = db;
        }

        public async Task<OrderStatusUpdatedDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // Load order (no need to include lines for status update)
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order is null)
                throw new NotFoundException($"Order '{request.Id}' was not found.");

            // Convert string to enum (validator ensures allowed values)
            var newStatus = Enum.Parse<OrderStatus>(request.Status);

            try
            {
                // Enforce domain transition rule
                order.ChangeStatus(newStatus);
            }
            catch (DomainException ex)
            {
                // Convert domain rule failure to 409 conflict
                throw new BusinessRuleException(ex.Message);
            }
            await _db.SaveChangesAsync(cancellationToken);
            return new OrderStatusUpdatedDto
            {
                Id = order.Id,
                Status = order.Status.ToString()
            };
        }
    }
}
