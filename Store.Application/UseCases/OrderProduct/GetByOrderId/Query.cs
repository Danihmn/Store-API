using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.OrderProduct.GetByOrderId;

public sealed record Query(Guid OrderId) : IQuery<IEnumerable<Response>>;
