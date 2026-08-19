using FluentResults;
using Store.Domain.Enums;
using Store.Domain.ValueObjects.Abstractions;

namespace Store.Domain.ValueObjects;

public record Status : ValueObject
{
    public EStatus Value { get; }

    private Status (EStatus value) => Value = value;

    public static Status FromPersistence (EStatus value) => new(value);

    public static Result<Status> Create (string value)
    {
        if (Enum.TryParse<EStatus>(value, true, out var status))
            return Result.Ok(new Status(status));

        return Result.Fail("Invalid status");
    }
}