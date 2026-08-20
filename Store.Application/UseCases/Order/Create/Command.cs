using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Order.Create;

public sealed record Command (decimal Total, Guid CustomerId, Guid AddressId) : ICommand<Response>;
