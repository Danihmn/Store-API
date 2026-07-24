using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Data.StoreContext;

namespace Store.Infrastructure.Repositories;

internal class CustomerRepository(StoreContext context) : ICustomerRepository
{
    public async Task<IEnumerable<Customer>?> GetAllAsync
        (int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        => await context.Customers
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Customer> CreateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        context.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Customer> UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                       ?? throw new KeyNotFoundException("Customer not found");

        context.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);
    }
}