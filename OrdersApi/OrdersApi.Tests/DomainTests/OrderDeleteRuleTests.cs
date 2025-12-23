using FluentAssertions;
using OrdersApi.Domain.Entities;
using OrdersApi.Domain.Enums;
using OrdersApi.Domain.Exceptions;

namespace OrdersApi.Tests.DomainTests
{
    public class OrderDeleteRuleTests
    {
        [Fact]
        public void EnsureCanDelete_WhenPending_ShouldNotThrow()
        {
            var order = new Order { Status = OrderStatus.Pending };

            Action act = () => order.EnsureCanDelete();

            act.Should().NotThrow();
        }

        [Fact]
        public void EnsureCanDelete_WhenConfirmed_ShouldThrow()
        {
            var order = new Order { Status = OrderStatus.Confirmed };

            Action act = () => order.EnsureCanDelete();

            act.Should().Throw<DomainException>();
        }
    }
}
