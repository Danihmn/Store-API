using FluentValidation;
using Store.Application.UseCases.Order.Update;
using Store.Domain.Enums;

namespace Store.Api.Validators.Order;

public sealed class OrderUpdateValidator : AbstractValidator<Command>
{
    public OrderUpdateValidator()
    {
        RuleFor(o => o.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(o => o.Status).Must(status => Enum.TryParse<EStatus>(status, true, out _))
            .WithMessage("Status is invalid")
            .When(o => o.Status is not null);
        RuleFor(o => o.Total).GreaterThan(0).WithMessage("Total must be greater than 0");
        RuleFor(o => o.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(o => o.AddressId).NotEmpty().WithMessage("AddressId is required");
    }
}
