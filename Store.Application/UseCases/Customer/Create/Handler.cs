using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.Create;

public sealed class Handler(ICustomerRepository repository, ILogger<Handler> logger)
    : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to create customer {Name}", request.Name);

        var customer = Domain.Entities.Customer.Create(request.Name, request.Email, request.Phone);

        if (customer.IsFailed)
        {
            logger.LogWarning("Failed to create customer {Name}: invalid data", request.Name);
            return Result.Fail<Response>(customer.Errors);
        }

        var result = await repository.CreateAsync(customer.Value, cancellationToken);

        logger.LogInformation("Created customer {Id}", customer.Value.Id);

        return Result.Ok(new Response(
            Id: result.Id,
            CreatedAt: result.CreatedAt,
            UpdatedAt: result.UpdatedAt,
            Name: result.Name,
            Email: result.Email.Value,
            Phone: result.Phone.Value));
    }
}