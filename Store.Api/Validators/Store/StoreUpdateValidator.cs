using FluentValidation;
using Store.Application.UseCases.StoreEntity.Update;

namespace Store.Api.Validators.Store;

public sealed class StoreUpdateValidator : AbstractValidator<Command>
{
    public StoreUpdateValidator()
    {
        RuleFor(s => s.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(s => s.LegalName).NotEmpty().WithMessage("LegalName is required");
        RuleFor(s => s.Cnpj).NotEmpty().WithMessage("Cnpj is required");
        RuleFor(s => s.Cnpj).Must(HaveValidDigitCount).WithMessage("Cnpj must have 11 (CPF) or 14 (CNPJ) digits")
            .When(s => !string.IsNullOrWhiteSpace(s.Cnpj));
        RuleFor(s => s.AddressId).NotEmpty().WithMessage("AddressId is required");
    }

    private static bool HaveValidDigitCount(string cnpj)
    {
        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        return digits.Length is 11 or 14;
    }
}
