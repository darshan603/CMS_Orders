using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Infrastructure.Persistence.Configurations
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            // Primary key
            builder.HasKey(ol => ol.Id);

            // Required product name
            builder.Property(ol => ol.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            // Money precision
            builder.Property(ol => ol.UnitPrice)
                .HasPrecision(18, 2);

            // Quantity should be required (validation also covers >0)
            builder.Property(ol => ol.Quantity)
                .IsRequired();
        }
    }
}
