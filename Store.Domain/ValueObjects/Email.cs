using EmailValidation;
using FluentResults;
using Store.Domain.ValueObjects.Abstractions;

namespace Store.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    private Email (string address) => Value = address;

    public static Email FromPersistence (string value) => new(value);

    public static Result<Email> Create (string address)
    {
        if (string.IsNullOrWhiteSpace(address) || !EmailValidator.Validate(address))
            return Result.Fail("Invalid email");

        return Result.Ok(new Email(address));
    }
}