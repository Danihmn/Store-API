using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.Create;

public sealed class Handler(IProductRepository repository, ILogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to create product {Description}", request.Description);

        var productResult = Domain.Entities.Product.Create(request.Description, request.UnitPrice, request.Stock);

        if (productResult.IsFailed)
        {
            logger.LogWarning("Failed to create product {Description} due to invalid data", request.Description);
            return Result.Fail<Response>(productResult.Errors);
        }

        var created = await repository.CreateAsync(productResult.Value, cancellationToken);

        logger.LogInformation("Created product {Id} - {Description}", created.Id, created.Description);

        return Result.Ok(new Response(
            Id: created.Id,
            CreatedAt: created.CreatedAt,
            UpdatedAt: created.UpdatedAt,
            Description: created.Description,
            UnitPrice: created.UnitPrice,
            Stock: created.Stock));
    }
}