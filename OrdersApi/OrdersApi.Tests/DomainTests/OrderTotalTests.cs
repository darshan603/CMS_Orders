using FluentAssertions;
using OrdersApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Tests.DomainTests
{
    public class OrderTotalTests
    {
        [Fact]
        public void RecalculateTotal_ShouldSumUnitPriceTimesQuantity()
        {
            // Arrange
            var order = new Order();
            order.Lines.Add(new OrderLine { ProductName = "A", UnitPrice = 10m, Quantity = 2 }); // 20
            order.Lines.Add(new OrderLine { ProductName = "B", UnitPrice = 5m, Quantity = 3 });  // 15

            // Act
            order.RecalculateTotal();

            // Assert
            order.TotalAmount.Should().Be(35m);
        }
    }
}
