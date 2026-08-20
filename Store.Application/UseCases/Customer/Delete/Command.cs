using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Customer.Delete;

public sealed record Command (Guid Id) : ICommand;
