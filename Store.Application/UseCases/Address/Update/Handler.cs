using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.Update;

public sealed class Handler (IAddressRepository repository, ILogger<Handler> logger) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating address {AddressId}", request.Id);

        var address = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (address is null)
        {
            logger.LogWarning("Update aborted, address {AddressId} was not found", request.Id);
            return Result.Fail<Response>("Address not found");
        }

        var updateResult = address.UpdateAddress(request.Street, request.City, request.State, request.ZipCode);

        if (updateResult.IsFailed)
        {
            logger.LogWarning("Update aborted, invalid data supplied for address {AddressId}", request.Id);
            return Result.Fail<Response>(updateResult.Errors);
        }

        var updated = await repository.UpdateAsync(address, cancellationToken);

        logger.LogInformation("Address {AddressId} updated successfully", updated.Id);

        return Result.Ok(new Response(
            Id: updated.Id,
            CreatedAt: updated.CreatedAt,
            UpdatedAt: updated.UpdatedAt,
            Street: updated.Street,
            City: updated.City,
            State: updated.State,
            ZipCode: updated.ZipCode.Value));
    }
}
