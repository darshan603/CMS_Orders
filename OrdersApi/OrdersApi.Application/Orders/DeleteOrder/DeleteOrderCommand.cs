using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public Guid Id { get; init; }
    }
}
