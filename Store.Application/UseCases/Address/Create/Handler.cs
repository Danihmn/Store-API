using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.Create;

public sealed class Handler (IAddressRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var addressResult = Store.Domain.Entities.Address.Create(request.Street, request.City, request.State, request.ZipCode);

        if (addressResult.IsFailed)
            return Result.Fail<Response>(addressResult.Errors);

        var created = await repository.CreateAsync(addressResult.Value, cancellationToken);

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
