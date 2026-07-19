using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.Delete;

public sealed class Handler (IOrderRepository repository) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
            return Result.Fail("Order not found");

        await repository.DeleteAsync(request.Id, cancellationToken);

        return Result.Ok();
    }
}
