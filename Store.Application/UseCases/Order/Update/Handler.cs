using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.Update;

public sealed class Handler (IOrderRepository repository, ILogger<Handler> logger) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating order {OrderId}", request.Id);

        var order = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found for update", request.Id);
            return Result.Fail<Response>("Order not found");
        }

        var updateResult = order.UpdateOrder(request.Status, request.Total);

        if (updateResult.IsFailed)
        {
            logger.LogWarning("Failed to apply updates to order {OrderId}", request.Id);
            return Result.Fail<Response>(updateResult.Errors);
        }

        var updated = await repository.UpdateAsync(order, cancellationToken);

        logger.LogInformation("Order {OrderId} updated successfully", updated.Id);

        return Result.Ok(new Response(
            Id: updated.Id,
            CreatedAt: updated.CreatedAt,
            UpdatedAt: updated.UpdatedAt,
            Status: updated.Status.Value.ToString(),
            Total: updated.Total.Value,
            CustomerId: updated.CustomerId,
            AddressId: updated.AddressId));
    }
}
