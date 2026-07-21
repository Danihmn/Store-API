using FluentValidation;
using Store.Application.UseCases.Order.Create;

namespace Store.Api.Validators.Order;

public sealed class OrderCreateValidator : AbstractValidator<Command>
{
    public OrderCreateValidator()
    {
        RuleFor(o => o.Total).GreaterThan(0).WithMessage("Total must be greater than 0");
        RuleFor(o => o.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(o => o.AddressId).NotEmpty().WithMessage("AddressId is required");
    }
}
