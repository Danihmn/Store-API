using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.Update;

public sealed class Handler(IOrderProductRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating order product for order {OrderId} and product {ProductId}", request.OrderId,
            request.ProductId);

        var item = await repository.GetByCompositeKeyAsync(request.OrderId, request.ProductId, cancellationToken);

        if (item is null)
        {
            logger.LogWarning("Cannot update: order product not found for order {OrderId} and product {ProductId}",
                request.OrderId, request.ProductId);
            return Result.Fail<Response>("Order product not found");
        }

        var updateResult = item.UpdateQuantity(request.Quantity);

        if (updateResult.IsFailed)
        {
            logger.LogWarning("Quantity update rejected for order {OrderId} and product {ProductId}", request.OrderId,
                request.ProductId);
            return Result.Fail<Response>(updateResult.Errors);
        }

        var updated = await repository.UpdateAsync(item, cancellationToken);

        logger.LogInformation("Order product quantity updated for order {OrderId} and product {ProductId}",
            updated.OrderId, updated.ProductId);

        return Result.Ok(new Response(
            OrderId: updated.OrderId,
            ProductId: updated.ProductId,
            Quantity: updated.Quantity));
    }
}