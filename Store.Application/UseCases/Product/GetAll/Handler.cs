using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.GetAll;

public sealed class Handler (IProductRepository repository) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle (Command request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(request.Skip, request.Take, cancellationToken);

        if (products is null || !products.Any())
            return Result.Fail<IEnumerable<Response>>("No products found");

        var responses = products.Select(p => new Response(
            Id: p.Id,
            CreatedAt: p.CreatedAt,
            UpdatedAt: p.UpdatedAt,
            Description: p.Description,
            UnitPrice: p.UnitPrice,
            Stock: p.Stock));

        return Result.Ok(responses);
    }
}
