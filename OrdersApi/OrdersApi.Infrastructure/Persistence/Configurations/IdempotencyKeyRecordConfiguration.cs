using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApi.Domain.Entities;

namespace OrdersApi.Infrastructure.Persistence.Configurations
{
    public class IdempotencyKeyRecordConfiguration : IEntityTypeConfiguration<IdempotencyKeyRecord>
    {
        public void Configure(EntityTypeBuilder<IdempotencyKeyRecord> builder)
        {
            // Primary key is the idempotency key string
            builder.HasKey(x => x.Key);

            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();
        }
    }
}
