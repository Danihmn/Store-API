using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.StoreEntity.GetById;

public sealed record Query(Guid Id) : IQuery<Response>;
