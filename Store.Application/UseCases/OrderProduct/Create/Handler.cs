using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.OrderProduct.Create;

public sealed class Handler (IOrderProductRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var itemResult = Store.Domain.Entities.OrderProduct.Create(request.OrderId, request.ProductId, request.Quantity);

        if (itemResult.IsFailed)
            return Result.Fail<Response>(itemResult.Errors);

        var created = await repository.CreateAsync(itemResult.Value, cancellationToken);

        return Result.Ok(new Response(
            OrderId: created.OrderId,
            ProductId: created.ProductId,
            Quantity: created.Quantity));
    }
}
