using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.GetById;

public sealed class Handler (ICustomerRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
            return Result.Fail<Response>("Customer not found");

        return Result.Ok(new Response(
            Id: customer.Id,
            CreatedAt: customer.CreatedAt,
            UpdatedAt: customer.UpdatedAt,
            Name: customer.Name,
            Email: customer.Email.Value,
            Phone: customer.Phone.Value));
    }
}
