using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Common.Models;
using OrdersApi.Application.Interfaces;
using OrdersApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.ListOrders
{
    public class ListOrdersHandler : IRequestHandler<ListOrdersQuery, PagedResult<OrderListItemDto>>
    {
        private readonly IOrdersDbContext _db;
        public ListOrdersHandler(IOrdersDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<OrderListItemDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            // Start with all orders (no Include(Lines) because list endpoint returns basic info only)
            var query = _db.Orders.AsNoTracking().AsQueryable();

            // --- Filtering ---
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                // Convert string -> enum (validator already ensures allowed values)
                var statusEnum = Enum.Parse<OrderStatus>(request.Status, ignoreCase: true);
                // Compare using enum value (EF Core can translate this)
                query = query.Where(o => o.Status == statusEnum);
            }
            if (request.FromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= request.ToDate.Value);
            }
            if (request.MinTotal.HasValue)
            {
                query = query.Where(o => o.TotalAmount == request.MinTotal.Value);
            }
            // --- Sorting ---
            var sort = (request.Sort ?? "orderDate").Trim().ToLowerInvariant();
            var dir = (request.Dir ?? "desc").ToLowerInvariant();

            query = (sort, dir) switch
            {
                ("totalamount", "asc") => query.OrderBy(o => o.TotalAmount),
                ("totalamount", "desc") => query.OrderByDescending(o => o.TotalAmount),
                ("orderdate", "asc") => query.OrderBy(o => o.OrderDate),
                _ => query.OrderByDescending(o => o.OrderDate)
            };

            // --- Pagination ---
            var totalCount = await query.CountAsync(cancellationToken);

            var page = request.Page;
            var pageSize = request.PageSize;
            var skip = (page - 1) * pageSize;

            var items = await query
            .Skip(skip)
            .Take(pageSize)
            .Select(o => new OrderListItemDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalAmount
            })
            .ToListAsync(cancellationToken);

            return new PagedResult<OrderListItemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
