using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Security.Services;

namespace Store.Application.UseCases.User.Create;

public class Handler(IUserRepository repository, ITokenService tokenService)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var newUser = Store.Domain.Entities.User.Create(request.Name, request.Email,
            PasswordService.HashPassword(request.Password), request.Active,
            request.Role);

        if (newUser.IsFailed)
            return Result.Fail<Response>(newUser.Errors);

        await repository.CreateAsync(newUser.Value, cancellationToken);

        return Result.Ok(new Response(newUser.Value.Id, tokenService.GenerateToken(newUser.Value),
            JwtBearerDefaults.AuthenticationScheme));
    }
}