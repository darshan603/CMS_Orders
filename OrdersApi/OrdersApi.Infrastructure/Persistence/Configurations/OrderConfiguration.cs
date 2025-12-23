using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApi.Domain.Entities;
using OrdersApi.Domain.Enums;

namespace OrdersApi.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary key
            builder.HasKey(o => o.Id);

            // Required customer name
            builder.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            // Store enum as string for readability in DB
            builder.Property(o => o.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v))
                .IsRequired();

            // Money precision
            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            // Relationship: Order (1) -> OrderLine (many)
            builder.HasMany(o => o.Lines)
                .WithOne()
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
