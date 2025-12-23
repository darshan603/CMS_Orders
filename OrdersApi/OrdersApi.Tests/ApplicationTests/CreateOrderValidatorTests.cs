using FluentAssertions;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Orders.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Tests.ApplicationTests
{
    public class CreateOrderValidatorTests
    {
        [Fact]
        public void CreateOrder_WithNegativeUnitPrice_ShouldFailValidation()
        {
            var validator = new CreateOrderValidator();

            var cmd = new CreateOrderCommand
            {
                CustomerName = "Test",
                Lines =
            {
                new CreateOrderLineDto
                {
                    ProductName = "Item",
                    UnitPrice = -10,
                    Quantity = 1
                }
            }
            };

            var result = validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName.Contains("UnitPrice"));
        }
    }
}
