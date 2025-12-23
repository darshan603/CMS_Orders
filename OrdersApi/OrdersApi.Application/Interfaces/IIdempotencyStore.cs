
namespace OrdersApi.Application.Interfaces
{
    public interface IIdempotencyStore
    {
        // Returns the existing OrderId if this key was used before
        Task<Guid?> GetOrderIdAsync(string key, CancellationToken ct);

        // Saves the mapping key -> orderId
        Task SaveAsync(string key, Guid orderId, CancellationToken ct);
    }
}
