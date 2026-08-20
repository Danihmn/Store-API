using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.GetByOrderId;

public sealed class Handler(IOrderProductRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, IEnumerable<Response>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching order products for order {OrderId}", request.OrderId);

        var items = await repository.GetByOrderIdAsync(request.OrderId, cancellationToken);
        var responses = (items ?? []).Select(item => new Response(
            OrderId: item.OrderId,
            ProductId: item.ProductId,
            Quantity: item.Quantity)).ToList();

        logger.LogInformation("Retrieved {Count} order products for order {OrderId}", responses.Count(),
            request.OrderId);

        return Result.Ok<IEnumerable<Response>>(responses);
    }
}