using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Order.Update;

public sealed record Command
    (Guid Id, string? Status, decimal Total, Guid CustomerId, Guid AddressId) : ICommand<Response>;
