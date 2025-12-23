using Microsoft.EntityFrameworkCore;
using OrdersApi.Application.Interfaces;
using OrdersApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Infrastructure.Persistence.Stores
{
    public class IdempotencyStore : IIdempotencyStore
    {
        private readonly OrdersDbContext _db;
        public IdempotencyStore(OrdersDbContext db)
        {
            _db = db;
        }
        public async Task<Guid?> GetOrderIdAsync(string key, CancellationToken ct)
        {
            var record = await _db.IdempotencyKeys
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Key == key, ct);

            return record?.OrderId;
        }

        public async Task SaveAsync(string key, Guid orderId, CancellationToken ct)
        {
            _db.IdempotencyKeys.Add(new IdempotencyKeyRecord
            {
                Key = key,
                OrderId = orderId,
                CreatedAtUtc = DateTime.UtcNow
            });

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                // Another request inserted the same key concurrently.             
            }
        }
    }
}
