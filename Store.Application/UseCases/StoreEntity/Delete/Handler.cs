using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.StoreEntity.Delete;

public sealed class Handler (IStoreRepository repository) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        var store = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (store is null)
            return Result.Fail("Store not found");

        await repository.DeleteAsync(request.Id, cancellationToken);

        return Result.Ok();
    }
}
