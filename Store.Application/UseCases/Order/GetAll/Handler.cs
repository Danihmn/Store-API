using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.GetAll;

public sealed class Handler(IOrderRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching orders with Skip {Skip} and Take {Take}", request.Skip, request.Take);

        var orders = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);
        var responses = (orders ?? []).Select(order => new Response(
            Id: order.Id,
            CreatedAt: order.CreatedAt,
            UpdatedAt: order.UpdatedAt,
            Status: order.Status.Value.ToString(),
            Total: order.Total.Value,
            CustomerId: order.CustomerId,
            AddressId: order.AddressId)).ToList();

        logger.LogInformation("Retrieved {Count} orders", responses.Count);

        return Result.Ok<IEnumerable<Response>>(responses);
    }
}