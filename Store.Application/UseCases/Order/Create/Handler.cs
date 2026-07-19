using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Order.Create;

public sealed class Handler (IOrderRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var orderResult = Store.Domain.Entities.Order.Create("pending", request.Total, request.CustomerId, request.AddressId);

        if (orderResult.IsFailed)
            return Result.Fail<Response>(orderResult.Errors);

        var created = await repository.CreateAsync(orderResult.Value, cancellationToken);

        return Result.Ok(new Response(
            Id: created.Id,
            CreatedAt: created.CreatedAt,
            UpdatedAt: created.UpdatedAt,
            Status: created.Status.Value.ToString(),
            Total: created.Total.Value,
            CustomerId: created.CustomerId,
            AddressId: created.AddressId));
    }
}
