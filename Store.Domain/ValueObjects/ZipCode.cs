using FluentResults;
using Store.Domain.ValueObjects.Abstractions;

namespace Store.Domain.ValueObjects;

public record ZipCode : ValueObject
{
    public string Value { get; }

    private ZipCode (string value) => Value = value;

    public static ZipCode FromPersistence (string value) => new(value);

    public static Result<ZipCode> Create (string value)
    {
        var cleaned = Clean(value);

        if (!IsValid(cleaned))
            return Result.Fail("Invalid zip code");

        return Result.Ok(new ZipCode(cleaned));
    }

    private static string Clean (string value) =>
        value?.Replace("-", "").Trim() ?? string.Empty;

    private static bool IsValid (string value) =>
        value.Length == 8 && value.All(char.IsDigit);
}