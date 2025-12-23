
namespace OrdersApi.Application.Common.Dtos
{
    public class OrderStatusUpdatedDto
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
