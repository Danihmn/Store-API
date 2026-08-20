using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.GetById;

public sealed class Handler(IProductRepository repository, ILogger<Handler> logger)
    : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Looking up product {Id}", request.Id);

        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product {Id} was not found", request.Id);
            return Result.Fail<Response>("Product not found");
        }

        logger.LogInformation("Found product {Id} - {Description}", product.Id, product.Description);

        return Result.Ok(new Response(
            Id: product.Id,
            CreatedAt: product.CreatedAt,
            UpdatedAt: product.UpdatedAt,
            Description: product.Description,
            UnitPrice: product.UnitPrice,
            Stock: product.Stock));
    }
}