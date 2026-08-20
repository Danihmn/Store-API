using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Product.GetById;

public sealed record Query(Guid Id) : IQuery<Response>;
