using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.GetAll;

public sealed class Handler (ICustomerRepository repository) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle (Command request, CancellationToken cancellationToken)
    {
        var customers = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);

        if (customers is null || !customers.Any())
            return Result.Fail<IEnumerable<Response>>("No customers found");

        var responses = customers.Select(customer => new Response(
            Id: customer.Id,
            CreatedAt: customer.CreatedAt,
            UpdatedAt: customer.UpdatedAt,
            Name: customer.Name,
            Email: customer.Email.Value,
            Phone: customer.Phone.Value));

        return Result.Ok(responses);
    }
}
