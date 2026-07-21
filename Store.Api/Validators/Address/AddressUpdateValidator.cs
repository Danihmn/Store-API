using FluentValidation;
using Store.Application.UseCases.Address.Update;

namespace Store.Api.Validators.Address;

public sealed class AddressUpdateValidator : AbstractValidator<Command>
{
    public AddressUpdateValidator()
    {
        RuleFor(a => a.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(a => a.Street).NotEmpty().WithMessage("Street is required");
        RuleFor(a => a.City).NotEmpty().WithMessage("City is required");
        RuleFor(a => a.State).NotEmpty().WithMessage("State is required");
        RuleFor(a => a.ZipCode).NotEmpty().WithMessage("ZipCode is required");
        RuleFor(a => a.ZipCode).Matches(@"^\d{5}-?\d{3}$").WithMessage("ZipCode must have 8 digits")
            .When(a => !string.IsNullOrWhiteSpace(a.ZipCode));
    }
}
