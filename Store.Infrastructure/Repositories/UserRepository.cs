using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Domain.ValueObjects;
using Store.Infrastructure.Data.StoreContext;

namespace Store.Infrastructure.Repositories;

public class UserRepository(StoreContext context) : IUserRepository
{
    public async Task<IEnumerable<User>?> GetAllAsync
        (int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailVo = Email.FromPersistence(email);

        return await context.Users
            .AsNoTracking()
            .Where(u => u.Email == emailVo)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        context.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<User> UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        context.Users.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found");

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}