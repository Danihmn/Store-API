using FluentResults;
using Store.Domain.Entities.Abstractions;
using Store.Domain.Validations;
using Store.Domain.ValueObjects;

namespace Store.Domain.Entities;

public class Address : Entity
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public ZipCode ZipCode { get; private set; }

    private Address(string street, string city, string state, ZipCode zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Address> Create(string street, string city, string state, string zipCode)
    {
        var errors = new List<IError>();

        errors.NotEmpty(street, "Street cannot be empty");
        errors.NotEmpty(city, "City cannot be empty");
        errors.NotEmpty(state, "State cannot be empty");

        if (state.Length > 2)
            errors.Add(new Error("State should only have the abbreviation"));

        var zipCodeResult = ZipCode.Create(zipCode);

        zipCodeResult.AddErrorsTo(errors);

        return errors.Count > 0
            ? Result.Fail<Address>(errors)
            : Result.Ok(new Address(street, city, state, zipCodeResult.Value));
    }

    public Result UpdateAddress(string? street = null, string? city = null, string? state = null,
        string? zipCode = null)
    {
        var errors = new List<IError>();
        var newZipCode = ZipCode;

        if (zipCode != null)
        {
            var zipCodeResult = ZipCode.Create(zipCode);

            zipCodeResult.AddErrorsTo(errors);

            if (zipCodeResult.IsSuccess) newZipCode = zipCodeResult.Value;
        }

        if (errors.Count > 0)
            return Result.Fail(errors);

        if (state is { Length: > 2 })
            errors.Add(new Error("State should only have the abbreviation"));

        Street = street ?? Street;
        City = city ?? City;
        State = state ?? State;
        ZipCode = newZipCode!;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}