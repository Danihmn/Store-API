using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.GetById;

public sealed class Handler (IOrderRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
            return Result.Fail<Response>("Order not found");

        return Result.Ok(new Response(
            Id: order.Id,
            CreatedAt: order.CreatedAt,
            UpdatedAt: order.UpdatedAt,
            Status: order.Status.Value.ToString(),
            Total: order.Total.Value,
            CustomerId: order.CustomerId,
            AddressId: order.AddressId));
    }
}
