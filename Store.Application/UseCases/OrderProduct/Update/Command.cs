using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.OrderProduct.Update;

public sealed record Command (Guid OrderId, Guid ProductId, int Quantity) : ICommand<Response>;
