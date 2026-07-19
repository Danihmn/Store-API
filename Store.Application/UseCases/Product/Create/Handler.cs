using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.Create;

public sealed class Handler (IProductRepository repository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle (Command request, CancellationToken cancellationToken)
    {
        var productResult = Store.Domain.Entities.Product.Create(request.Description, request.UnitPrice, request.Stock);

        if (productResult.IsFailed)
            return Result.Fail<Response>(productResult.Errors);

        var created = await repository.CreateAsync(productResult.Value, cancellationToken);

        return Result.Ok(new Response(
            Id: created.Id,
            CreatedAt: created.CreatedAt,
            UpdatedAt: created.UpdatedAt,
            Description: created.Description,
            UnitPrice: created.UnitPrice,
            Stock: created.Stock));
    }
}
