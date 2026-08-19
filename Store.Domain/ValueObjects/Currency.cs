using FluentResults;
using Store.Domain.ValueObjects.Abstractions;

namespace Store.Domain.ValueObjects;

public record Currency : ValueObject
{
    public decimal Value { get; }

    private Currency (decimal amount) => Value = amount;

    public static Currency FromPersistence (decimal value) => new(value);

    public static Result<Currency> Create (decimal amount)
    {
        if (amount <= 0)
            return Result.Fail("Amount must be greater than 0");

        return Result.Ok(new Currency(amount));
    }
}