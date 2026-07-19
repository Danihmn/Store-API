using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.Update;

public sealed class Handler (IOrderProductRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByCompositeKeyAsync(request.OrderId, request.ProductId, cancellationToken);

        if (item is null)
            return Result.Fail<Response>("Order product not found");

        var updateResult = item.UpdateQuantity(request.Quantity);

        if (updateResult.IsFailed)
            return Result.Fail<Response>(updateResult.Errors);

        var updated = await repository.UpdateAsync(item, cancellationToken);

        return Result.Ok(new Response(
            OrderId: updated.OrderId,
            ProductId: updated.ProductId,
            Quantity: updated.Quantity));
    }
}
