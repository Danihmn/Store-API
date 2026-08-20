using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.OrderProduct.Delete;

public sealed record Command (Guid OrderId, Guid ProductId) : ICommand;
