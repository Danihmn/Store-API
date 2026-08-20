using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Order.GetAll;

public sealed record Query(int Skip = 0, int Take = 10) : IQuery<IEnumerable<Response>>;
