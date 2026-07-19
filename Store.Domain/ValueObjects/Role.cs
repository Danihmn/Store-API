using FluentResults;
using Store.Domain.Abstractions;
using Store.Domain.Enums;

namespace Store.Domain.ValueObjects;

public class Role : ValueObject
{
    public string Value { get; }
    public ERole Type { get; }

    private Role(string value, ERole type)
    {
        Value = value;
        Type = type;
    }

    public static Role FromPersistence(string value) => new(value, Enum.Parse<ERole>(value, ignoreCase: true));

    public static Result<Role> Create(string value)
    {
        ERole type;

        switch (value.ToLower())
        {
            case "admin":
                type = ERole.Admin;
                break;
            case "seller":
                type = ERole.Seller;
                break;
            case "purchaser":
                type = ERole.Purchaser;
                break;
            case "stock_clerk":
                type = ERole.StockClerk;
                break;
            default:
                return Result.Fail<Role>("Invalid role");
        }


        return Result.Ok(new Role(value.ToLower(), type));
    }
}