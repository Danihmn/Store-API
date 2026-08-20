using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.StoreEntity.GetAll;

public sealed record Query(int Skip = 0, int Take = 10) : IQuery<IEnumerable<Response>>;
