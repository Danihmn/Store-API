using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Order.Delete;

public sealed record Command (Guid Id) : ICommand;
