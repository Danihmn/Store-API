using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.Create;

public sealed class Handler(IOrderProductRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating order product for order {OrderId} and product {ProductId}", request.OrderId,
            request.ProductId);

        var itemResult =
            Domain.Entities.OrderProduct.Create(request.OrderId, request.ProductId, request.Quantity);

        if (itemResult.IsFailed)
        {
            logger.LogWarning("Could not create order product for order {OrderId}: invalid data supplied",
                request.OrderId);
            return Result.Fail<Response>(itemResult.Errors);
        }

        var created = await repository.CreateAsync(itemResult.Value, cancellationToken);

        logger.LogInformation("Order product created for order {OrderId} and product {ProductId}", created.OrderId,
            created.ProductId);

        return Result.Ok(new Response(
            OrderId: created.OrderId,
            ProductId: created.ProductId,
            Quantity: created.Quantity));
    }
}