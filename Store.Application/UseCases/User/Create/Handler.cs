using FluentResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Security.Services;

namespace Store.Application.UseCases.User.Create;

public class Handler(IUserRepository repository, ITokenService tokenService, ILogger<Handler> logger)
    : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to create user");

        var existing = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (existing != null)
        {
            logger.LogWarning("Email already exists");
            return Result.Fail("Email already exists");
        }

        var newUser = Domain.Entities.User.Create(request.Name, request.Email,
            PasswordService.HashPassword(request.Password), request.Active,
            request.Role);

        if (newUser.IsFailed)
        {
            logger.LogWarning("Failed to create user");
            return Result.Fail<Response>(newUser.Errors);
        }

        await repository.CreateAsync(newUser.Value, cancellationToken);

        logger.LogInformation("Created User: {User}", newUser.Value.Email.Value);

        return Result.Ok(new Response(newUser.Value.Id, tokenService.GenerateToken(newUser.Value),
            JwtBearerDefaults.AuthenticationScheme));
    }
}