using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.Update;

public sealed class Handler (IStoreRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var store = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (store is null)
            return Result.Fail<Response>("Store not found");

        var updateResult = store.UpdateStore(request.LegalName, request.TradeName, request.Cnpj, request.Active, request.AddressId);

        if (updateResult.IsFailed)
            return Result.Fail<Response>(updateResult.Errors);

        var updated = await repository.UpdateAsync(store, cancellationToken);

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
