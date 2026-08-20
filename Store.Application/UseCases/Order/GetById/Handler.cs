using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.GetById;

public sealed class Handler(IOrderRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching order {OrderId}", request.Id);

        var order = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} was not found", request.Id);
            return Result.Fail<Response>("Order not found");
        }

        logger.LogInformation("Order {OrderId} retrieved successfully", request.Id);

        return Result.Ok(new Response(
            Id: order.Id,
            CreatedAt: order.CreatedAt,
            UpdatedAt: order.UpdatedAt,
            Status: order.Status.Value.ToString(),
            Total: order.Total.Value,
            CustomerId: order.CustomerId,
            AddressId: order.AddressId));
    }
}