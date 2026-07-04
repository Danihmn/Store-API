using FluentResults;
using Store.Domain.Abstractions;
using Store.Domain.Enums;

namespace Store.Domain.ValueObjects;

public class Role : ValueObject
{
    public string Value { get; }
    public ERole Type { get; }

    private Role (string value, ERole type)
    {
        Value = value;
        Type = type;
    }

    public static Role FromPersistence (string value) => new(value, Enum.Parse<ERole>(value));

    public static Result<Role> Create (string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Enum.TryParse<ERole>(value, out var type))
            return Result.Fail("Invalid role");

        return Result.Ok(new Role(value, type));
    }
}