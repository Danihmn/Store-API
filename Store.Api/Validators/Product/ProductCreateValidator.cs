using FluentValidation;
using Store.Application.UseCases.Product.Create;

namespace Store.Api.Validators.Product;

public sealed class ProductCreateValidator : AbstractValidator<Command>
{
    public ProductCreateValidator()
    {
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.UnitPrice).GreaterThan(0).WithMessage("UnitPrice must be greater than 0");
        RuleFor(p => p.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative")
            .When(p => p.Stock is not null);
    }
}