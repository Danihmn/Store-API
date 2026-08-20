using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Address.GetById;

public sealed record Query(Guid Id) : IQuery<Response>;