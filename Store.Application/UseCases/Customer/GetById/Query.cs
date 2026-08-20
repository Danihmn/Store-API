using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Customer.GetById;

public sealed record Query(Guid Id) : IQuery<Response>;
