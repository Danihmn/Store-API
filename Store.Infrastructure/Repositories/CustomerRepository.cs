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
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Customer> UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        var customer = context.Customers.FirstOrDefault(c => c.Id == entity.Id)
                       ?? throw new KeyNotFoundException("Customer not found");

        await context.AddAsync(customer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = context.Customers.FirstOrDefault(c => c.Id == id)
                       ?? throw new KeyNotFoundException("Customer not found");

        context.Customers.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);
    }
}