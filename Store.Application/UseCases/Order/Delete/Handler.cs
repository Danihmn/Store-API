using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.Delete;

public sealed class Handler(IOrderRepository repository, ILogger<Handler> logger) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete order {OrderId}", request.Id);

        var order = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found for deletion", request.Id);
            return Result.Fail("Order not found");
        }

        await repository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Order {OrderId} deleted successfully", request.Id);

        return Result.Ok();
    }
}