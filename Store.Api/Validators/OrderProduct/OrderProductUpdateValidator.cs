using FluentValidation;
using Store.Application.UseCases.OrderProduct.Update;

namespace Store.Api.Validators.OrderProduct;

public sealed class OrderProductUpdateValidator : AbstractValidator<Command>
{
    public OrderProductUpdateValidator()
    {
        RuleFor(op => op.OrderId).NotEmpty().WithMessage("OrderId is required");
        RuleFor(op => op.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(op => op.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
