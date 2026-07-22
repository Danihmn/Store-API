using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Customer.GetAll;

public sealed class Handler (ICustomerRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching customers with Skip {Skip} and Take {Take}", request.Skip, request.Take);

        var customers = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);

        if (customers is null || !customers.Any())
        {
            logger.LogWarning("No customers found for Skip {Skip} and Take {Take}", request.Skip, request.Take);
            return Result.Fail<IEnumerable<Response>>("No customers found");
        }

        var responses = customers.Select(customer => new Response(
            Id: customer.Id,
            CreatedAt: customer.CreatedAt,
            UpdatedAt: customer.UpdatedAt,
            Name: customer.Name,
            Email: customer.Email.Value,
            Phone: customer.Phone.Value)).ToList();

        logger.LogInformation("Returning {Count} customers", responses.Count);

        return Result.Ok<IEnumerable<Response>>(responses);
    }
}
