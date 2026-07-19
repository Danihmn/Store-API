using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.Delete;

public sealed class Handler (IOrderProductRepository repository) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByCompositeKeyAsync(request.OrderId, request.ProductId, cancellationToken);

        if (item is null)
            return Result.Fail("Order product not found");

        await repository.DeleteByCompositeKeyAsync(request.OrderId, request.ProductId, cancellationToken);

        return Result.Ok();
    }
}
