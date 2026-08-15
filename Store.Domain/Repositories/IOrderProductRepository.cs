using Store.Domain.Entities;
using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface IOrderProductRepository :
    IAllReadableRepository<OrderProduct>,
    ICreatableRepository<OrderProduct>,
    IUpdatableRepository<OrderProduct>

{
    Task<IEnumerable<OrderProduct>?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<OrderProduct?> GetByCompositeKeyAsync(Guid orderId, Guid productId,
        CancellationToken cancellationToken = default);

    Task DeleteByCompositeKeyAsync(Guid orderId, Guid productId, CancellationToken cancellationToken = default);
}