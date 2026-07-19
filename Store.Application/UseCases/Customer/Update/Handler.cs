using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.Update;

public sealed class Handler (ICustomerRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
            return Result.Fail<Response>("Customer not found");

        var updateResult = customer.UpdateCustomer(request.Name, request.Email, request.Phone);

        if (updateResult.IsFailed)
            return Result.Fail<Response>(updateResult.Errors);

        var updated = await repository.UpdateAsync(customer, cancellationToken);

        return Result.Ok(new Response(
            Id: updated.Id,
            CreatedAt: updated.CreatedAt,
            UpdatedAt: updated.UpdatedAt,
            Name: updated.Name,
            Email: updated.Email.Value,
            Phone: updated.Phone.Value));
    }
}
