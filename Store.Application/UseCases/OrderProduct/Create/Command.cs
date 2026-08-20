using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.OrderProduct.Create;

public sealed record Command (Guid OrderId, Guid ProductId, int Quantity) : ICommand<Response>;
