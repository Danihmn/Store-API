using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.Delete;

public sealed class Handler (IOrderProductRepository repository, ILogger<Handler> logger) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete order product for order {OrderId} and product {ProductId}", request.OrderId, request.ProductId);

        var item = await repository.GetByCompositeKeyAsync(request.OrderId, request.ProductId, cancellationToken);

        if (item is null)
        {
            logger.LogWarning("Order product not found for order {OrderId} and product {ProductId}", request.OrderId, request.ProductId);
            return Result.Fail("Order product not found");
        }

        await repository.DeleteByCompositeKeyAsync(request.OrderId, request.ProductId, cancellationToken);

        logger.LogInformation("Order product removed from order {OrderId} and product {ProductId}", request.OrderId, request.ProductId);

        return Result.Ok();
    }
}
