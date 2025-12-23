using OrdersApi.Domain.Enums;
using OrdersApi.Domain.Exceptions;

namespace OrdersApi.Domain.Entities
{
    public class Order
    {     
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CustomerName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal TotalAmount { get; private set; }

        public List<OrderLine> Lines { get; set; } = new();

        public DateTime CreateOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Recalculates the total amount of the order
        /// based on unit price and quantity of each order line
        /// </summary>
        public void RecalculateTotal()
        {
            TotalAmount = Lines.Sum(line => line.UnitPrice * line.Quantity);
        }

        /// <summary>
        /// Changes the order status while enforcing allowed transitions
        /// Allowed:
        /// Pending → Confirmed
        /// Pending → Cancelled
        /// </summary>
        /// <param name="newStatus">Target status</param>
        /// <exception cref="DomainException">Thrown when transition is invalid</exception>
        public void ChangeStatus(OrderStatus newStatus)
        {
            if (Status == OrderStatus.Pending &&
                (newStatus == OrderStatus.Confirmed || newStatus == OrderStatus.Cancelled))
            {
                Status = newStatus;
                return;
            }

            throw new DomainException($"Invalid status change from {Status} to {newStatus}");
        }

        /// <summary>
        /// Ensures that the order can be deleted
        /// Only orders in Pending status are allowed to be deleted
        /// </summary>
        /// <exception cref="DomainException">Thrown if deletion is not allowed</exception>
        public void EnsureCanDelete()
        {
            if (Status != OrderStatus.Pending)
            {
                throw new DomainException("Only orders with Pending status can be deleted.");
            }
        }
    }
}
