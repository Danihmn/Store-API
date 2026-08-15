using Store.Domain.Entities;
using Store.Domain.Repositories.Abstractions;

namespace Store.Domain.Repositories;

public interface ICustomerRepository :
    IAllReadableRepository<Customer>,
    IByIdReadableRepository<Customer>,
    ICreatableRepository<Customer>,
    IUpdatableRepository<Customer>,
    IDeletableRepository;