using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.GetById;

public sealed class Handler(IStoreRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Looking up store {Id}", request.Id);

        var store = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (store is null)
        {
            logger.LogWarning("Store {Id} was not found", request.Id);
            return Result.Fail<Response>("Store not found");
        }

        logger.LogInformation("Retrieved store {Id}", request.Id);

        return Result.Ok(new Response(
            Id: store.Id,
            CreatedAt: store.CreatedAt,
            UpdatedAt: store.UpdatedAt,
            LegalName: store.LegalName,
            TradeName: store.TradeName,
            Cnpj: store.Cnpj.Value,
            Active: store.Active,
            AddressId: store.AddressId));
    }
}