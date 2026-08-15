namespace Store.Domain.Repositories.Abstractions;

public interface IAllReadableRepository<T> where T : class
{
    Task<IEnumerable<T>?> GetAllAsync(int skip = 0, int take = 10, CancellationToken cancellationToken = default);
}

public interface IByIdReadableRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICreatableRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
}

public interface IUpdatableRepository<T> where T : class
{
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
}

public interface IDeletableRepository
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}