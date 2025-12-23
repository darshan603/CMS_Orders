using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Interfaces;
using OrdersApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Infrastructure.Persistence
{
    public class OrdersDbContext : DbContext, IOrdersDbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderLine> OrderLines => Set<OrderLine>();

        public DbSet<IdempotencyKeyRecord> IdempotencyKeys => Set<IdempotencyKeyRecord>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
