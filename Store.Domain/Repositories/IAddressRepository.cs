using Store.Domain.Entities;
using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface IAddressRepository :
    IAllReadableRepository<Address>,
    IByIdReadableRepository<Address>,
    ICreatableRepository<Address>,
    IUpdatableRepository<Address>,
    IDeletableRepository;