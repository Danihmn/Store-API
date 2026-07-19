using FluentResults;
using Store.Domain.Abstractions;
using Store.Domain.Validations;
using Store.Domain.ValueObjects;

namespace Store.Domain.Entities;

public class User : Entity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string HashedPassword { get; private set; }
    public bool Active { get; private set; }
    public Role Role { get; private set; }

    private User(string name, Email email, string hashedPassword, bool active, Role role)
    {
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        Active = active;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<User> Create(string name, string email, string hashedPassword, bool? active, string role)
    {
        var errors = new List<IError>();

        errors.NotEmpty(name, "InvalidName", "Name cannot be empty");
        errors.NotEmpty(hashedPassword, "InvalidPassword", "Password cannot be empty");

        var emailResult = Email.Create(email);
        var roleResult = Role.Create(role);

        emailResult.AddErrorsTo(errors);
        roleResult.AddErrorsTo(errors);

        if (errors.Count > 0) return Result.Fail<User>(errors);

        return Result.Ok(new User(name, emailResult.Value, hashedPassword, active ?? true, roleResult.Value));
    }

    public Result UpdateUser(string? name = null, string? email = null, string? hashedPassword = null,
        bool? active = null, string? role = null)
    {
        var errors = new List<IError>();

        Email? emailValue = null;
        Role? roleValue = null;

        if (email != null)
        {
            var emailResult = Email.Create(email);

            emailResult.AddErrorsTo(errors);
            emailValue = emailResult.IsFailed ? null : emailResult.Value;
        }

        if (role != null)
        {
            var roleResult = Role.Create(role);

            roleResult.AddErrorsTo(errors);
            roleValue = roleResult.IsFailed ? null : roleResult.Value;
        }

        errors.NotEmptyIfProvided(name, "InvalidName", "Name cannot be empty");
        errors.NotEmptyIfProvided(hashedPassword, "InvalidPassword", "Password cannot be empty");

        if (errors.Count > 0) return Result.Fail(errors);

        Name = name ?? Name;
        Email = emailValue ?? Email;
        HashedPassword = hashedPassword ?? HashedPassword;
        Active = active ?? Active;
        Role = roleValue ?? Role;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}