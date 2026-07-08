using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Security.Services;

namespace Store.Application.UseCases.User.Authenticate;

public class Handler (IUserRepository repository, ITokenService tokenService) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !PasswordService.IsValidPassword(request.Password, user.HashedPassword))
            return Result.Fail(new Error("Invalid credentials"));

        return Result.Ok(new Response(Token: tokenService.Create(user), Type: JwtBearerDefaults.AuthenticationScheme));
    }
}