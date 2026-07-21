using FluentValidation;
using Store.Application.UseCases.Customer.Update;

namespace Store.Api.Validators.Customer;

public sealed class CustomerUpdateValidator : AbstractValidator<Command>
{
    public CustomerUpdateValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(c => c.Email).EmailAddress().WithMessage("Email is invalid")
            .When(c => !string.IsNullOrWhiteSpace(c.Email));
        RuleFor(c => c.Phone).NotEmpty().WithMessage("Phone can't be empty")
            .When(c => c.Phone is not null);
    }
}
