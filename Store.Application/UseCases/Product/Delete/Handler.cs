using FluentResults;
using MediatR;
using Store.Domain.Repositories;

namespace Store.Application.UseCases.Product.Delete;

public sealed class Handler (IProductRepository repository) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle (Command request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result.Fail("Product not found");

        await repository.DeleteAsync(request.Id, cancellationToken);

        return Result.Ok();
    }
}
