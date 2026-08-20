using FluentResults;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Messaging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.Update;

public sealed class Handler(IProductRepository repository, ILogger<Handler> logger)
    : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting update for product {Id}", request.Id);

        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Cannot update product {Id} because it was not found", request.Id);
            return Result.Fail<Response>("Product not found");
        }

        var updateResult = product.UpdateProduct(request.Description, request.UnitPrice, request.Stock);

        if (updateResult.IsFailed)
        {
            logger.LogWarning("Failed to update product {Id} due to invalid data", request.Id);
            return Result.Fail<Response>(updateResult.Errors);
        }

        var updated = await repository.UpdateAsync(product, cancellationToken);

        logger.LogInformation("Updated product {Id} - {Description}", updated.Id, updated.Description);

        return Result.Ok(new Response(
            Id: updated.Id,
            CreatedAt: updated.CreatedAt,
            UpdatedAt: updated.UpdatedAt,
            Description: updated.Description,
            UnitPrice: updated.UnitPrice,
            Stock: updated.Stock));
    }
}