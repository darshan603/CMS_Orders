using FluentValidation;
using OrdersApi.Application.Common.Dtos;

namespace OrdersApi.Application.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            // Customer name cannot be empty
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("CustomerName is required.")
                .MaximumLength(200).WithMessage("CustomerName cannot exceed 200 characters.");

            // Must have at least one order line
            RuleFor(x => x.Lines)
                .NotNull().WithMessage("Lines are required.")
                .Must(lines => lines.Count > 0).WithMessage("At least one order line is required.");

            // Validate each line
            RuleForEach(x => x.Lines).SetValidator(new CreateOrderLineValidator());
        }
        /// <summary>
        /// Validates a single order line in the Create Order request.
        /// </summary>
        public class CreateOrderLineValidator : AbstractValidator<CreateOrderLineDto>
        {
            public CreateOrderLineValidator()
            {
                RuleFor(x => x.ProductName)
                    .NotEmpty().WithMessage("ProductName is required.")
                    .MaximumLength(200).WithMessage("ProductName cannot exceed 200 characters.");

                RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");

                RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            }
        }
    }
}
