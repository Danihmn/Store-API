using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Order.GetById;

public sealed record Query(Guid Id) : IQuery<Response>;
