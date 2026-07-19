using MediatR;
using Microsoft.AspNetCore.Authorization;
using Create = Store.Application.UseCases.Order.Create;
using Delete = Store.Application.UseCases.Order.Delete;
using GetAll = Store.Application.UseCases.Order.GetAll;
using GetById = Store.Application.UseCases.Order.GetById;
using Update = Store.Application.UseCases.Order.Update;

namespace Store.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapGet("", async (ISender sender, CancellationToken cancellationToken, int skip = 0, int take = 10) =>
        {
            var result = await sender.Send(new GetAll.Command(skip, take), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization("admin", "seller");

        group.MapGet("{id:Guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetById.Command(id), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization();

        group.MapPost("", async (Create.Command command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess
                ? Results.Created($"/orders/{result.Value.Id}", result.Value)
                : Results.BadRequest(new
                {
                    errors = result.Errors.Select(error => error.Message),
                });
        }).RequireAuthorization("admin", "seller");

        group.MapPut("{id:Guid}", async
            (Guid id, Update.Command command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command with { Id = id }, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization("admin", "seller");

        group.MapDelete("{id:Guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new Delete.Command(id), cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result);
        }).RequireAuthorization("admin");
    }
}