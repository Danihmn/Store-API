using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Create = Store.Application.UseCases.Customer.Create;
using Delete = Store.Application.UseCases.Customer.Delete;
using GetAll = Store.Application.UseCases.Customer.GetAll;
using GetById = Store.Application.UseCases.Customer.GetById;
using Update = Store.Application.UseCases.Customer.Update;

namespace Store.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers").WithTags("Customers");

        group.MapGet("", async (ISender sender, CancellationToken cancellationToken, int skip = 0, int take = 10) =>
        {
            var result = await sender.Send(new GetAll.Query(skip, take), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization(policy => policy.RequireRole("admin", "seller"));

        group.MapGet("{id:Guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetById.Query(id), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization(policy => policy.RequireRole("admin", "seller"));

        group.MapPost("", async
        (Create.Command command, ISender sender, IValidator<Create.Command> validator,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
                return Results.BadRequest(validation.ToDictionary());

            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess
                ? Results.Created($"/customers/{result.Value.Id}", result.Value)
                : Results.BadRequest(new
                {
                    errors = result.Errors.Select(error => error.Message),
                });
        }).RequireAuthorization(policy => policy.RequireRole("admin", "seller"));

        group.MapPut("{id:Guid}", async
        (Guid id, Update.Command command, ISender sender, IValidator<Update.Command> validator,
            CancellationToken cancellationToken) =>
        {
            var updated = command with { Id = id };
            var validation = await validator.ValidateAsync(updated, cancellationToken);

            if (!validation.IsValid)
                return Results.BadRequest(validation.ToDictionary());

            var result = await sender.Send(updated, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result);
        }).RequireAuthorization(policy => policy.RequireRole("admin", "seller"));

        group.MapDelete("{id:Guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new Delete.Command(id), cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result);
        }).RequireAuthorization(policy => policy.RequireRole("admin"));
    }
}