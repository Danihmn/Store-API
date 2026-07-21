using FluentValidation;
using Store.Application.UseCases.Customer.Create;

namespace Store.Api.Validators.Customer;

public sealed class CustomerCreateValidator : AbstractValidator<Command>
{
    public CustomerCreateValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(c => c.Email).EmailAddress().WithMessage("Email is invalid")
            .When(c => !string.IsNullOrWhiteSpace(c.Email));
        RuleFor(c => c.Phone).NotEmpty().WithMessage("Phone is required");
    }
}
