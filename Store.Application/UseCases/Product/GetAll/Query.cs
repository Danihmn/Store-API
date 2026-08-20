using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Product.GetAll;

public sealed record Query(int Skip = 0, int Take = 10) : IQuery<IEnumerable<Response>>;
