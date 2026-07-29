using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.GetById;

public sealed class Handler(ICustomerRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Looking up customer {Id}", request.Id);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning("Customer {Id} not found", request.Id);
            return Result.Fail<Response>("Customer not found");
        }

        logger.LogInformation("Found customer {Id}", request.Id);

        return Result.Ok(new Response(
            Id: customer.Id,
            CreatedAt: customer.CreatedAt,
            UpdatedAt: customer.UpdatedAt,
            Name: customer.Name,
            Email: customer.Email.Value,
            Phone: customer.Phone.Value));
    }
}