using FluentResults;
using Store.Domain.Abstractions;
using Store.Domain.Validations;
using Store.Domain.ValueObjects;

namespace Store.Domain.Entities;

public class Customer : Entity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }

    private Customer(string name, Email email, Phone phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Customer> Create(string name, string email, string phone)
    {
        var errors = new List<IError>();

        errors.NotEmpty(name, "Name cannot be empty");

        var emailResult = Email.Create(email);
        var phoneResult = Phone.Create(phone);

        emailResult.AddErrorsTo(errors);
        phoneResult.AddErrorsTo(errors);

        return errors.Count > 0
            ? Result.Fail<Customer>(errors)
            : Result.Ok(new Customer(name, emailResult.Value, phoneResult.Value));
    }

    public Result UpdateCustomer(string? name = null, string? email = null, string? phone = null)
    {
        var errors = new List<IError>();

        Email? emailValue = null;
        Phone? phoneValue = null;

        if (email != null)
        {
            var emailResult = Email.Create(email);

            emailResult.AddErrorsTo(errors);
            emailValue = emailResult.IsFailed ? null : emailResult.Value;
        }

        if (phone != null)
        {
            var phoneResult = Phone.Create(phone);

            phoneResult.AddErrorsTo(errors);
            phoneValue = phoneResult.IsFailed ? null : phoneResult.Value;
        }

        errors.NotEmptyIfProvided(name, "Name cannot be empty");

        if (errors.Count > 0) return Result.Fail(errors);

        Name = name ?? Name;
        Email = emailValue ?? Email;
        Phone = phoneValue ?? Phone;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}