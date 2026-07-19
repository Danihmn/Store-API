using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.Create;

public sealed class CreateHandler(ICustomerRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var customer = Domain.Entities.Customer.Create(request.Name, request.Email, request.Phone);

        if (customer.IsFailed)
            return Result.Fail<Response>(customer.Errors);

        await repository.CreateAsync(customer.Value, cancellationToken);

        return Result.Ok(new Response(
            Id: customer.Value.Id,
            CreatedAt: customer.Value.CreatedAt,
            UpdatedAt: customer.Value.UpdatedAt,
            Name: customer.Value.Name,
            Email: customer.Value.Email.Value,
            Phone: customer.Value.Phone.Value));
    }
}