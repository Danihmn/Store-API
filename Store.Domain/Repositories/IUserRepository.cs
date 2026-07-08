using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync (string email, CancellationToken cancellationToken = default);
}