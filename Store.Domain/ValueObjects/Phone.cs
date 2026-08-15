using FluentResults;
using PhoneNumbers;
using Store.Domain.ValueObjects.Abstractions;

namespace Store.Domain.ValueObjects;

public class Phone : ValueObject
{
    public string Value { get; }

    private Phone (string number) => Value = number;

    public static Phone FromPersistence (string number) => new(number);

    public static Result<Phone> Create (string number)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(number) || !IsValid(number))
                return Result.Fail("Invalid phone number");

            return Result.Ok(new Phone(number));
        }
        catch (NumberParseException)
        {
            return Result.Fail("Invalid phone number");
        }
        catch
        {
            return Result.Fail("Error trying to create phone number");
        }
    }

    private static bool IsValid (string number)
    {
        var util = PhoneNumbers.PhoneNumberUtil.GetInstance();
        var phoneNumber = util.Parse(number, null);

        return util.IsValidNumber(phoneNumber);
    }
}