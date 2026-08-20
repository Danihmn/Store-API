using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.GetById;

public sealed class Handler(IAddressRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Looking up address {AddressId}", request.Id);

        var address = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (address is null)
        {
            logger.LogWarning("Address {AddressId} was not found", request.Id);
            return Result.Fail<Response>("Address not found");
        }

        logger.LogInformation("Address {AddressId} retrieved successfully", request.Id);

        return Result.Ok(new Response(
            Id: address.Id,
            CreatedAt: address.CreatedAt,
            UpdatedAt: address.UpdatedAt,
            Street: address.Street,
            City: address.City,
            State: address.State,
            ZipCode: address.ZipCode.Value));
    }
}