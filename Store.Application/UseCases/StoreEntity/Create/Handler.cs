using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.Create;

public sealed class Handler (IStoreRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var storeResult = Store.Domain.Entities.Store.Create(
            request.LegalName,
            request.Cnpj,
            request.AddressId,
            request.TradeName,
            request.Active);

        if (storeResult.IsFailed)
            return Result.Fail<Response>(storeResult.Errors);

        var created = await repository.CreateAsync(storeResult.Value, cancellationToken);

        return Result.Ok(new Response(
            Id: created.Id,
            CreatedAt: created.CreatedAt,
            UpdatedAt: created.UpdatedAt,
            LegalName: created.LegalName,
            TradeName: created.TradeName,
            Cnpj: created.Cnpj.Value,
            Active: created.Active,
            AddressId: created.AddressId));
    }
}
