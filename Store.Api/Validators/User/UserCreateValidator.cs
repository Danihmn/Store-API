using FluentValidation;
using Store.Application.UseCases.User.Create;

namespace Store.Api.Validators.User;

public sealed class UserCreateValidator : AbstractValidator<Command>
{
    public UserCreateValidator()
    {
        RuleFor(u => u.Name).NotNull().WithMessage("Name is required");
        RuleFor(u => u.Name).NotEmpty().WithMessage("Name can't be empty");
        RuleFor(u => u.Email).NotNull().WithMessage("Email is required");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email can't be empty");
        RuleFor(u => u.Password).NotNull().WithMessage("Password is required");
        RuleFor(u => u.Password).MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}