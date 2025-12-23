
namespace OrdersApi.Application.Common.Dtos
{
    public class OrderDetailsDto
    {
        public Guid Id { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public DateTime OrderDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal TotalAmount { get; init; }
        public List<OrderLineDetailsDto> Lines { get; init; } = new();
    }
}
