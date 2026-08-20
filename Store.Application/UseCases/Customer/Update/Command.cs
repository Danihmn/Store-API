using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Customer.Update;

public sealed record Command (Guid Id, string Name, string Email, string? Phone) : ICommand<Response>;
