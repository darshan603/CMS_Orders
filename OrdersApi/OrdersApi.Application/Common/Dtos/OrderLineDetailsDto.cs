
namespace OrdersApi.Application.Common.Dtos
{
    public class OrderLineDetailsDto
    {
        public Guid Id { get; init; }
        public Guid OrderId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
    }
}
