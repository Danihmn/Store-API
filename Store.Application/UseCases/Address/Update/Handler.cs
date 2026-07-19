using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.Update;

public sealed class Handler (IAddressRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var address = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (address is null)
            return Result.Fail<Response>("Address not found");

        var updateResult = address.UpdateAddress(request.Street, request.City, request.State, request.ZipCode);

        if (updateResult.IsFailed)
            return Result.Fail<Response>(updateResult.Errors);

        var updated = await repository.UpdateAsync(address, cancellationToken);

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
