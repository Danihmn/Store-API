using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.StoreEntity.Update;

public sealed record Command
    (Guid Id, string LegalName, string? TradeName, string Cnpj, bool Active, Guid AddressId) : ICommand<Response>;
