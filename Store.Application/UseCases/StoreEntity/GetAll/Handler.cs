using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.GetAll;

public sealed class Handler(IStoreRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, IEnumerable<Response>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching stores with Skip {Skip} and Take {Take}", request.Skip, request.Take);

        var stores = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);
        var responses = (stores ?? []).Select(store => new Response(
            Id: store.Id,
            CreatedAt: store.CreatedAt,
            UpdatedAt: store.UpdatedAt,
            LegalName: store.LegalName,
            TradeName: store.TradeName,
            Cnpj: store.Cnpj.Value,
            Active: store.Active,
            AddressId: store.AddressId)).ToList();

        logger.LogInformation("Returning {Count} stores", responses.Count);

        return Result.Ok<IEnumerable<Response>>(responses);
    }
}