
namespace OrdersApi.Application.Common.Dtos
{
    public class CreateOrderLineDto
    {
        public string ProductName { get; init; } = string.Empty;
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
    }
}
