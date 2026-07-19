using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Data.StoreContext;

namespace Store.Infrastructure.Repositories;

internal class AddressRepository(StoreContext context) : IAddressRepository
{
    public async Task<IEnumerable<Address>?> GetAllAsync
        (int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        => await context.Addresses
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<Address> CreateAsync(Address entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Address> UpdateAsync(Address entity, CancellationToken cancellationToken = default)
    {
        context.Addresses.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
                      ?? throw new KeyNotFoundException("Address not found");

        context.Addresses.Remove(address);
        await context.SaveChangesAsync(cancellationToken);
    }
}