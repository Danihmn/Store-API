using Store.Domain.Entities;
using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface IOrderRepository :
    IAllReadableRepository<Order>,
    IByIdReadableRepository<Order>,
    ICreatableRepository<Order>,
    IUpdatableRepository<Order>,
    IDeletableRepository;