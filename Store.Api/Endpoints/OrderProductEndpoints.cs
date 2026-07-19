using MediatR;
using Microsoft.AspNetCore.Authorization;
using Create = Store.Application.UseCases.OrderProduct.Create;
using Delete = Store.Application.UseCases.OrderProduct.Delete;
using GetByOrderId = Store.Application.UseCases.OrderProduct.GetByOrderId;
using Update = Store.Application.UseCases.OrderProduct.Update;

namespace Store.Api.Endpoints;

public static class OrderProductEndpoints
{
    public static void MapOrderProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders/{orderId:Guid}/items").WithTags("Order Items");

        group.MapGet("", async (Guid orderId, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetByOrderId.Command(orderId), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization();

        group.MapPost("",
            async (Guid orderId, Create.Command command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command with { OrderId = orderId }, cancellationToken);
                return result.IsSuccess
                    ? Results.Created($"/orders/{orderId}/items/{result.Value.ProductId}", result.Value)
                    : Results.BadRequest(new
                    {
                        errors = result.Errors.Select(error => error.Message),
                    });
            }).RequireAuthorization("admin", "seller");

        group.MapPut("{productId:Guid}", async
        (Guid orderId, Guid productId, Update.Command command, ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command with { OrderId = orderId, ProductId = productId },
                cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization("admin", "seller");

        group.MapDelete("{productId:Guid}", async
            (Guid orderId, Guid productId, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new Delete.Command(orderId, productId), cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result);
        }).RequireAuthorization("admin", "seller");
    }
}