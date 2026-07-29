using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.Delete;

public sealed class Handler(ICustomerRepository repository, ILogger<Handler> logger) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete customer {Id}", request.Id);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning("Cannot delete customer {Id}: not found", request.Id);
            return Result.Fail("Customer not found");
        }

        await repository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Deleted customer {Id}", request.Id);

        return Result.Ok();
    }
}