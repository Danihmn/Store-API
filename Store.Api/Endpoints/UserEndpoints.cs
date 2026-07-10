using MediatR;
using Store.Application.UseCases.User.Authenticate;

namespace Store.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapPost("authenticate", async
            (ISender sender, CancellationToken cancellationToken, string email, string password) =>
        {
            var result = await sender.Send(new Command(email, password), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });
    }
}