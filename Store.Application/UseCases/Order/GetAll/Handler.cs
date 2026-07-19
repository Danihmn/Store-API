using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.GetAll;

public sealed class Handler (IOrderRepository repository) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle (Command request, CancellationToken cancellationToken)
    {
        var orders = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);

        if (orders is null || !orders.Any())
            return Result.Fail<IEnumerable<Response>>("No orders found");

        var responses = orders.Select(order => new Response(
            Id: order.Id,
            CreatedAt: order.CreatedAt,
            UpdatedAt: order.UpdatedAt,
            Status: order.Status.Value.ToString(),
            Total: order.Total.Value,
            CustomerId: order.CustomerId,
            AddressId: order.AddressId));

        return Result.Ok(responses);
    }
}
