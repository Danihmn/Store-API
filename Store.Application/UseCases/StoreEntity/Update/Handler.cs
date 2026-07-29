using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.Update;

public sealed class Handler(IStoreRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting update for store {Id}", request.Id);

        var store = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (store is null)
        {
            logger.LogWarning("Cannot update store {Id} because it was not found", request.Id);
            return Result.Fail<Response>("Store not found");
        }

        var updateResult = store.UpdateStore(request.LegalName, request.TradeName, request.Cnpj, request.Active,
            request.AddressId);

        if (updateResult.IsFailed)
        {
            logger.LogWarning("Failed to update store {Id}: {Errors}", request.Id,
                string.Join(", ", updateResult.Errors));
            return Result.Fail<Response>(updateResult.Errors);
        }

        var updated = await repository.UpdateAsync(store, cancellationToken);

        logger.LogInformation("Updated store {Id}", updated.Id);

        return Result.Ok(new Response(
            Id: updated.Id,
            CreatedAt: updated.CreatedAt,
            UpdatedAt: updated.UpdatedAt,
            LegalName: updated.LegalName,
            TradeName: updated.TradeName,
            Cnpj: updated.Cnpj.Value,
            Active: updated.Active,
            AddressId: updated.AddressId));
    }
}