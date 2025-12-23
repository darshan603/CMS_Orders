using FluentAssertions;
using OrdersApi.Domain.Entities;
using OrdersApi.Domain.Enums;
using OrdersApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Tests.DomainTests
{
    public class OrderStatusTests
    {
        [Fact]
        public void ChangeStatus_PendingToConfirmed_ShouldWork()
        {
            var order = new Order { Status = OrderStatus.Pending };

            order.ChangeStatus(OrderStatus.Confirmed);

            order.Status.Should().Be(OrderStatus.Confirmed);
        }

        [Fact]
        public void ChangeStatus_ConfirmedToCancelled_ShouldThrow()
        {
            var order = new Order { Status = OrderStatus.Confirmed };

            Action act = () => order.ChangeStatus(OrderStatus.Cancelled);

            act.Should().Throw<DomainException>();
        }
    }
}
