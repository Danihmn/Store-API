using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.GetAll;

public sealed class Handler (IStoreRepository repository) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle (Command request, CancellationToken cancellationToken)
    {
        var stores = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);

        if (stores is null || !stores.Any())
            return Result.Fail<IEnumerable<Response>>("No stores found");

        var responses = stores.Select(store => new Response(
            Id: store.Id,
            CreatedAt: store.CreatedAt,
            UpdatedAt: store.UpdatedAt,
            LegalName: store.LegalName,
            TradeName: store.TradeName,
            Cnpj: store.Cnpj.Value,
            Active: store.Active,
            AddressId: store.AddressId));

        return Result.Ok(responses);
    }
}
