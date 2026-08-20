using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.Update;

public sealed class Handler(ICustomerRepository repository, ILogger<Handler> logger)
    : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to update customer {Id}", request.Id);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning("Cannot update customer {Id}: not found", request.Id);
            return Result.Fail<Response>("Customer not found");
        }

        var updateResult = customer.UpdateCustomer(request.Name, request.Email, request.Phone);

        if (updateResult.IsFailed)
        {
            logger.LogWarning("Failed to update customer {Id}: invalid data", request.Id);
            return Result.Fail<Response>(updateResult.Errors);
        }

        var updated = await repository.UpdateAsync(customer, cancellationToken);

        logger.LogInformation("Updated customer {Id}", updated.Id);

        return Result.Ok(new Response(
            Id: updated.Id,
            CreatedAt: updated.CreatedAt,
            UpdatedAt: updated.UpdatedAt,
            Name: updated.Name,
            Email: updated.Email.Value,
            Phone: updated.Phone.Value));
    }
}