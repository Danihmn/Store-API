using Store.Domain.Entities;
using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface IProductRepository :
    IAllReadableRepository<Product>,
    IByIdReadableRepository<Product>,
    ICreatableRepository<Product>,
    IUpdatableRepository<Product>,
    IDeletableRepository;