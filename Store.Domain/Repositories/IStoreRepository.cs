using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface IStoreRepository :
    IAllReadableRepository<Entities.Store>,
    IByIdReadableRepository<Entities.Store>,
    ICreatableRepository<Entities.Store>,
    IUpdatableRepository<Entities.Store>,
    IDeletableRepository;