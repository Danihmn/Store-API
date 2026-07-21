using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Store.Api.Extensions.Endpoints;
using Authenticate = Store.Application.UseCases.User.Authenticate;
using Create = Store.Application.UseCases.User.Create;

namespace Store.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapGet("me", (ClaimsPrincipal user) =>
            new
            {
                Id = user.Id(),
                Name = user.Name(),
                Email = user.Email(),
                Role = user.Role()
            });

        group.MapPost("authenticate", async
        (ISender sender, CancellationToken cancellationToken, Authenticate.Command command,
            IValidator<Authenticate.Command> validator) =>
        {
            var validation = await validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
                return Results.BadRequest(validation.ToDictionary());

            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });

        group.MapPost("",
            async (ISender sender, CancellationToken cancellationToken, Create.Command command,
                IValidator<Create.Command> validator) =>
            {
                var validation = await validator.ValidateAsync(command, cancellationToken);

                if (!validation.IsValid)
                    return Results.BadRequest(validation.ToDictionary());

                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(new
                    {
                        errors = result.Errors.Select(error => error.Message),
                    });
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "admin,seller" });
    }
}