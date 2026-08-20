using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Customer.Create;

public sealed record Command (string Name, string Email, string Phone) : ICommand<Response>;
