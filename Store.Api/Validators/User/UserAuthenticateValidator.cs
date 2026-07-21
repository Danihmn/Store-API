using FluentValidation;
using Store.Application.UseCases.User.Authenticate;

namespace Store.Api.Validators.User;

public sealed class UserAuthenticateValidator : AbstractValidator<Command>
{
    public UserAuthenticateValidator()
    {
        RuleFor(u => u.Email).NotNull().WithMessage("Email is required");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email can't be empty");
        RuleFor(u => u.Password).NotNull().WithMessage("Password is required");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password can't be empty");
    }
}