using MediatR;
using OrdersApi.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderDetailsDto>
    {
        public Guid Id { get; init; }
    }
}
