using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.Delete;

public sealed class Handler (IProductRepository repository, ILogger<Handler> logger) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete product {Id}", request.Id);

        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Cannot delete product {Id} because it was not found", request.Id);
            return Result.Fail("Product not found");
        }

        await repository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Deleted product {Id}", request.Id);

        return Result.Ok();
    }
}
