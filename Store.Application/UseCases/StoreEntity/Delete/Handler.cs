using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.Delete;

public sealed class Handler(IStoreRepository repository, ILogger<Handler> logger) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete store {Id}", request.Id);

        var store = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (store is null)
        {
            logger.LogWarning("Cannot delete store {Id} because it was not found", request.Id);
            return Result.Fail("Store not found");
        }

        await repository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Deleted store {Id}", request.Id);

        return Result.Ok();
    }
}