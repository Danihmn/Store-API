using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Data.StoreContext;

namespace Store.Infrastructure.Repositories;

internal class OrderProductRepository(StoreContext context) : IOrderProductRepository
{
    public async Task<IEnumerable<OrderProduct>?> GetAllAsync
        (int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        => await context.OrderProducts
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

    public Task<OrderProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("OrderProduct uses composite key. Use GetByCompositeKeyAsync.");

    public async Task<IEnumerable<OrderProduct>?> GetByOrderIdAsync
        (Guid orderId, CancellationToken cancellationToken = default)
        => await context.OrderProducts
            .AsNoTracking()
            .Where(op => op.OrderId == orderId)
            .ToListAsync(cancellationToken);

    public async Task<OrderProduct?> GetByCompositeKeyAsync
        (Guid orderId, Guid productId, CancellationToken cancellationToken = default)
        => await context.OrderProducts
            .AsNoTracking()
            .FirstOrDefaultAsync(op => op.OrderId == orderId && op.ProductId == productId, cancellationToken);

    public async Task<OrderProduct> CreateAsync(OrderProduct entity, CancellationToken cancellationToken = default)
    {
        context.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<OrderProduct> UpdateAsync(OrderProduct entity, CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            throw new NotSupportedException("OrderProduct uses composite key. Use DeleteByCompositeKeyAsync.");
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }

    public async Task DeleteByCompositeKeyAsync
        (Guid orderId, Guid productId, CancellationToken cancellationToken = default)
    {
        var item = await context.OrderProducts
                       .FirstOrDefaultAsync(op => op.OrderId == orderId && op.ProductId == productId, cancellationToken)
                   ?? throw new KeyNotFoundException("Order product not found");

        context.Remove(item);
        await context.SaveChangesAsync(cancellationToken);
    }
}