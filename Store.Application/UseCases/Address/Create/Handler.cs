using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.Create;

public sealed class Handler (IAddressRepository repository, ILogger<Handler> logger) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to create address in {City}/{State}", request.City, request.State);

        var addressResult = Domain.Entities.Address.Create(request.Street, request.City, request.State, request.ZipCode);

        if (addressResult.IsFailed)
        {
            logger.LogWarning("Address creation rejected due to invalid data for {City}/{State}", request.City, request.State);
            return Result.Fail<Response>(addressResult.Errors);
        }

        var created = await repository.CreateAsync(addressResult.Value, cancellationToken);

        logger.LogInformation("Address {AddressId} created successfully", created.Id);

        return Result.Ok(new Response(
            Id: created.Id,
            CreatedAt: created.CreatedAt,
            UpdatedAt: created.UpdatedAt,
            Street: created.Street,
            City: created.City,
            State: created.State,
            ZipCode: created.ZipCode.Value));
    }
}
