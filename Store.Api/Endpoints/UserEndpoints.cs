using MediatR;
using Microsoft.AspNetCore.Mvc;
using Authenticate = Store.Application.UseCases.User.Authenticate;
using Create = Store.Application.UseCases.User.Create;

namespace Store.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapPost("authenticate", async
            (ISender sender, CancellationToken cancellationToken, string email, string password) =>
        {
            var result = await sender.Send(new Authenticate.Command(email, password), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });

        group.MapPost("",
            async (ISender sender, CancellationToken cancellationToken, [FromBody] Create.Command command) =>
            {
                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(new
                    {
                        errors = result.Errors.Select(error => error.Message),
                    });
            }).RequireAuthorization("admin");
    }
}