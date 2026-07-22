using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Security.Services;

namespace Store.Application.UseCases.User.Authenticate;

public class Handler(IUserRepository repository, ITokenService tokenService, ILogger<Handler> logger)
    : IRequestHandler<Command,
        Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Entering Authenticate Handler with user {User}", request.Email);

        var user = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !PasswordService.IsValidPassword(request.Password, user.HashedPassword))
        {
            logger.LogWarning("User with {Email} is unauthorized", request.Email);
            return Result.Fail(new Error("Invalid credentials"));
        }

        if (user.Active)
        {
            logger.LogInformation("{User} is already active", request.Email);

            return Result.Ok(new Response(tokenService.GenerateToken(user),
                JwtBearerDefaults.AuthenticationScheme));
        }

        logger.LogWarning("{User} is not active", request.Email);

        return Result.Fail(new Error("User is not active"));
    }
}