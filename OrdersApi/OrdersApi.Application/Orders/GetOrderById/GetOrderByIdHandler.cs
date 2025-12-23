using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Common.Exceptions;
using OrdersApi.Application.Interfaces;

namespace OrdersApi.Application.Orders.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailsDto>
    {
        private readonly IOrdersDbContext _db;
        public GetOrderByIdHandler(IOrdersDbContext db)
        {
            _db = db;
        }
        public async Task<OrderDetailsDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // Load order + its lines
            var order = await _db.Orders
                .Include(o => o.Lines)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            // Return 404 if not found
            if (order is null)
                throw new NotFoundException($"Order '{request.Id}' was not found.");

            // Map to response DTO
            return new OrderDetailsDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                Lines = order.Lines.Select(line => new OrderLineDetailsDto
                {
                    Id = line.Id,
                    OrderId = line.OrderId,
                    ProductName = line.ProductName,
                    UnitPrice = line.UnitPrice,
                    Quantity = line.Quantity
                }).ToList()
            };
        }
    }
}
