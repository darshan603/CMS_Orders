using MediatR;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Interfaces;
using OrdersApi.Domain.Entities;
using OrdersApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDetailsDto>
    {
        private readonly IOrdersDbContext _db;
        public CreateOrderHandler(IOrdersDbContext db)
        {
            _db = db;
        }
        public async Task<OrderDetailsDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Build the Order entity
            var order = new Order
            {
                CustomerName = request.CustomerName,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Lines = request.Lines.Select(l => new OrderLine
                {
                    ProductName = l.ProductName,
                    UnitPrice = l.UnitPrice,
                    Quantity = l.Quantity
                }).ToList()
            };
            // Calculate total amount using domain logic
            order.RecalculateTotal();

            // Add order and save
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(cancellationToken);

            // Return response DTO
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
                    OrderId = order.Id,
                    ProductName = line.ProductName,
                    UnitPrice = line.UnitPrice,
                    Quantity = line.Quantity
                }).ToList()
            };
        }
    }
}
