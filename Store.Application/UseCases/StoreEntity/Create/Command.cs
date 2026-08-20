using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.StoreEntity.Create;

public sealed record Command
    (string LegalName, string? TradeName, string Cnpj, bool Active, Guid AddressId) : ICommand<Response>;
