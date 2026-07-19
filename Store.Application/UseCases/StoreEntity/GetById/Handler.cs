using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.GetById;

public sealed class Handler (IStoreRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var store = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (store is null)
            return Result.Fail<Response>("Store not found");

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
