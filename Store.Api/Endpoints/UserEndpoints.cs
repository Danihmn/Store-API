using MediatR;
using Authenticate = Store.Application.UseCases.User.Authenticate;
using Create = Store.Application.UseCases.User.Create;

namespace Store.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapPost("authenticate", async
            (ISender sender, CancellationToken cancellationToken, Authenticate.Command command) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });

        group.MapPost("",
            async (ISender sender, CancellationToken cancellationToken, Create.Command command) =>
            {
                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(new
                    {
                        errors = result.Errors.Select(error => error.Message),
                    });
            }).RequireAuthorization("admin", "seller");
    }
}