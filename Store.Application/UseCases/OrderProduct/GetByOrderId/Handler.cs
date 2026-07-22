using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.GetByOrderId;

public sealed class Handler (IOrderProductRepository repository, ILogger<Handler> logger) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching order products for order {OrderId}", request.OrderId);

        var items = await repository.GetByOrderIdAsync(request.OrderId, cancellationToken);

        if (items is null || !items.Any())
        {
            logger.LogWarning("No order products found for order {OrderId}", request.OrderId);
            return Result.Fail<IEnumerable<Response>>("No items found for this order");
        }

        var responses = items.Select(item => new Response(
            OrderId: item.OrderId,
            ProductId: item.ProductId,
            Quantity: item.Quantity));

        logger.LogInformation("Retrieved {Count} order products for order {OrderId}", responses.Count(), request.OrderId);

        return Result.Ok(responses);
    }
}
