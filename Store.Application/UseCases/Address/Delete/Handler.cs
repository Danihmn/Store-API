using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.Delete;

public sealed class Handler (IAddressRepository repository, ILogger<Handler> logger) : ICommandHandler<Command>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request to delete address {AddressId}", request.Id);

        var address = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (address is null)
        {
            logger.LogWarning("Cannot delete address {AddressId} because it was not found", request.Id);
            return Result.Fail("Address not found");
        }

        await repository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Address {AddressId} deleted successfully", request.Id);

        return Result.Ok();
    }
}
