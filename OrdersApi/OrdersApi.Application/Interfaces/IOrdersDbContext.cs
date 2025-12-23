using Microsoft.EntityFrameworkCore;
using OrdersApi.Domain.Entities;

namespace OrdersApi.Application.Interfaces
{
    public interface IOrdersDbContext
    {
        // Orders table
        DbSet<Order> Orders { get; }

        // Order lines table
        DbSet<OrderLine> OrderLines { get; }

        // Save changes to database
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
