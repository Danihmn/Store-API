using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Address.Delete;

public sealed record Command (Guid Id) : ICommand;
