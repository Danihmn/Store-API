using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.GetAll;

public sealed class Handler(IProductRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching products with Skip {Skip} and Take {Take}", request.Skip, request.Take);

        var products = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);
        var responses = (products ?? []).Select(p => new Response(
            Id: p.Id,
            CreatedAt: p.CreatedAt,
            UpdatedAt: p.UpdatedAt,
            Description: p.Description,
            UnitPrice: p.UnitPrice,
            Stock: p.Stock)).ToList();

        logger.LogInformation("Retrieved {Count} products", responses.Count);

        return Result.Ok<IEnumerable<Response>>(responses);
    }
}