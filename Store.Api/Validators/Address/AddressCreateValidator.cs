using FluentValidation;
using Store.Application.UseCases.Address.Create;

namespace Store.Api.Validators.Address;

public sealed class AddressCreateValidator : AbstractValidator<Command>
{
    public AddressCreateValidator()
    {
        RuleFor(a => a.Street).NotEmpty().WithMessage("Street is required");
        RuleFor(a => a.City).NotEmpty().WithMessage("City is required");
        RuleFor(a => a.State).NotEmpty().WithMessage("State is required");
        RuleFor(a => a.State).Length(2).WithMessage("Type just the abbreviation");
        RuleFor(a => a.ZipCode).NotEmpty().WithMessage("ZipCode is required");
        RuleFor(a => a.ZipCode).Matches(@"^\d{5}-?\d{3}$").WithMessage("ZipCode must have 8 digits")
            .When(a => !string.IsNullOrWhiteSpace(a.ZipCode));
    }
}