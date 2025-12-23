
namespace OrdersApi.Domain.Entities
{
    public class IdempotencyKeyRecord
    {
        // Idempotency key from request header (primary key)
        public string Key { get; set; } = string.Empty;

        // The order id created for this key
        public Guid OrderId { get; set; }

        // For auditing/debugging
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
