using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.Delete;

public sealed class Handler (IAddressRepository repository) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        var address = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (address is null)
            return Result.Fail("Address not found");

        await repository.DeleteAsync(request.Id, cancellationToken);

        return Result.Ok();
    }
}
