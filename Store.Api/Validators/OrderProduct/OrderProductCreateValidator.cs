using FluentValidation;
using Store.Application.UseCases.OrderProduct.Create;

namespace Store.Api.Validators.OrderProduct;

public sealed class OrderProductCreateValidator : AbstractValidator<Command>
{
    public OrderProductCreateValidator()
    {
        RuleFor(op => op.OrderId).NotEmpty().WithMessage("OrderId is required");
        RuleFor(op => op.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(op => op.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
