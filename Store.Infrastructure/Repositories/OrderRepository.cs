using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Data.StoreContext;

namespace Store.Infrastructure.Repositories;

internal class OrderRepository(StoreContext context) : IOrderRepository
{
    public async Task<IEnumerable<Order>?> GetAllAsync
        (int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        => await context.Orders
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<Order> CreateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Order> UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        context.Orders.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
                    ?? throw new KeyNotFoundException("Order not found");

        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
    }
}