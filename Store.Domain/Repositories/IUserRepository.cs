using Store.Domain.Entities;
using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface IUserRepository :
    IAllReadableRepository<User>,
    IByIdReadableRepository<User>,
    ICreatableRepository<User>,
    IUpdatableRepository<User>,
    IDeletableRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}