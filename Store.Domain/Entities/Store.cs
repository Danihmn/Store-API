using FluentResults;
using Store.Domain.Abstractions;
using Store.Domain.Validations;
using Store.Domain.ValueObjects;

namespace Store.Domain.Entities;

public class Store : Entity
{
    public string LegalName { get; private set; }
    public string? TradeName { get; private set; }
    public Document Cnpj { get; private set; }
    public bool Active { get; private set; }
    public Guid AddressId { get; private set; }

    private Store(string legalName, Document cnpj, Guid addressId, string? tradeName, bool active)
    {
        LegalName = legalName;
        TradeName = tradeName;
        Cnpj = cnpj;
        Active = active;
        AddressId = addressId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Store> Create(string legalName, string cnpj, Guid addressId, string? tradeName = null,
        bool active = true)
    {
        var errors = new List<IError>();

        errors.NotEmpty(legalName, "InvalidLegalName", "LegalName cannot be empty");
        errors.NotEmpty(addressId, "InvalidAddressId", "AddressId cannot be empty");

        var cnpjResult = Document.Create(cnpj);

        cnpjResult.AddErrorsTo(errors);

        return errors.Count > 0
            ? Result.Fail<Store>(errors)
            : Result.Ok(new Store(legalName, cnpjResult.Value, addressId, tradeName, active));
    }

    public Result UpdateStore(string? legalName = null, string? tradeName = null, string? cnpj = null,
        bool? active = null, Guid? addressId = null)
    {
        var errors = new List<IError>();
        Document newCnpj = Cnpj;

        errors.NotEmptyIfProvided(legalName, "InvalidLegalName", "LegalName cannot be empty");
        errors.NotEmptyIfProvided(addressId, "InvalidAddressId", "AddressId cannot be empty");

        if (cnpj != null)
        {
            var cnpjResult = Document.Create(cnpj);
            cnpjResult.AddErrorsTo(errors);

            if (cnpjResult.IsSuccess) newCnpj = cnpjResult.Value;
        }

        if (errors.Count > 0) return Result.Fail(errors);

        LegalName = legalName ?? LegalName;
        TradeName = tradeName ?? TradeName;
        Active = active ?? Active;
        AddressId = addressId ?? AddressId;
        Cnpj = newCnpj;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}