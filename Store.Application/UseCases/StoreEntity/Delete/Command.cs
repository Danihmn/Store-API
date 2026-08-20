using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.StoreEntity.Delete;

public sealed record Command (Guid Id) : ICommand;
