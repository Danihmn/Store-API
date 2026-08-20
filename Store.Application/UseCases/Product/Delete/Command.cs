using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Product.Delete;

public sealed record Command (Guid Id) : ICommand;
