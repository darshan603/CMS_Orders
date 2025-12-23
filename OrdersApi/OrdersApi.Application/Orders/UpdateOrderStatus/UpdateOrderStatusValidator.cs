using FluentValidation;

namespace OrdersApi.Application.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => s is "Pending" or "Confirmed" or "Cancelled")
                .WithMessage("Status must be one of: Pending, Confirmed, Cancelled.");
        }
    }
}
