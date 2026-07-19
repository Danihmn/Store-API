using Microsoft.EntityFrameworkCore;
using Store.Domain.Repositories;
using Store.Infrastructure.Data.StoreContext;
using StoreEntity = Store.Domain.Entities.Store;

namespace Store.Infrastructure.Repositories;

internal class StoreRepository(StoreContext context) : IStoreRepository
{
    public async Task<IEnumerable<StoreEntity>?> GetAllAsync
        (int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        => await context.Stores
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<StoreEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Stores
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<StoreEntity> CreateAsync(StoreEntity entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<StoreEntity> UpdateAsync(StoreEntity entity, CancellationToken cancellationToken = default)
    {
        context.Stores.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var store = await context.Stores.FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
                    ?? throw new KeyNotFoundException("Store not found");

        context.Stores.Remove(store);
        await context.SaveChangesAsync(cancellationToken);
    }
}