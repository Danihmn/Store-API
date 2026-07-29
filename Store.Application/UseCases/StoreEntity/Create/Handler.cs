using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.Create;

public sealed class Handler(IStoreRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to create store {LegalName}", request.LegalName);

        var storeResult = Domain.Entities.Store.Create(
            request.LegalName,
            request.Cnpj,
            request.AddressId,
            request.TradeName,
            request.Active);

        if (storeResult.IsFailed)
        {
            logger.LogWarning("Failed to create store {LegalName}: {Errors}", request.LegalName,
                string.Join(", ", storeResult.Errors));
            return Result.Fail<Response>(storeResult.Errors);
        }

        var created = await repository.CreateAsync(storeResult.Value, cancellationToken);

        logger.LogInformation("Created store {Id} with legal name {LegalName}", created.Id, created.LegalName);

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