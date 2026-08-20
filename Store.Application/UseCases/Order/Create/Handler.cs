using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.Create;

public sealed class Handler(IOrderRepository repository, ILogger<Handler> logger)
    : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to create order for customer {CustomerId}", request.CustomerId);

        var orderResult = Domain.Entities.Order.Create("pending", request.Total, request.CustomerId, request.AddressId);

        if (orderResult.IsFailed)
        {
            logger.LogWarning("Failed to create order for customer {CustomerId}", request.CustomerId);
            return Result.Fail<Response>(orderResult.Errors);
        }

        var created = await repository.CreateAsync(orderResult.Value, cancellationToken);

        logger.LogInformation("Created order {OrderId} for customer {CustomerId}", created.Id, created.CustomerId);

        return Result.Ok(new Response(
            Id: created.Id,
            CreatedAt: created.CreatedAt,
            UpdatedAt: created.UpdatedAt,
            Status: created.Status.Value.ToString(),
            Total: created.Total.Value,
            CustomerId: created.CustomerId,
            AddressId: created.AddressId));
    }
}