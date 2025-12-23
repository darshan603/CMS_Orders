using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApi.Application.Orders.ListOrders
{
    public class ListOrdersValidator : AbstractValidator<ListOrdersQuery>
    {
        public ListOrdersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("pageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("pageSize cannot exceed 100.");

            RuleFor(x => x.Sort)
                .Must(s => s is null or "orderDate" or "totalAmount")
                .WithMessage("sort must be one of: orderDate, totalAmount.");

            RuleFor(x => x.Dir)
                .Must(d => d is null or "asc" or "desc")
                .WithMessage("dir must be one of: asc, desc.");

            RuleFor(x => x.Status)
                .Must(s => s is null or "Pending" or "Confirmed" or "Cancelled")
                .WithMessage("status must be one of: Pending, Confirmed, Cancelled.");
        }
    }
}
