using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Address.GetAll;

public sealed class Handler(IAddressRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, IEnumerable<Response>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching addresses with Skip {Skip} and Take {Take}", request.Skip, request.Take);

        var addresses = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);
        var responses = (addresses ?? []).Select(a => new Response(
            Id: a.Id,
            CreatedAt: a.CreatedAt,
            UpdatedAt: a.UpdatedAt,
            Street: a.Street,
            City: a.City,
            State: a.State,
            ZipCode: a.ZipCode.Value));

        logger.LogInformation("Retrieved {Count} address(es)", responses.Count());

        return Result.Ok(responses);
    }
}