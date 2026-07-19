using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.Delete;

public sealed class Handler (ICustomerRepository repository) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
            return Result.Fail("Customer not found");

        await repository.DeleteAsync(request.Id, cancellationToken);

        return Result.Ok();
    }
}
