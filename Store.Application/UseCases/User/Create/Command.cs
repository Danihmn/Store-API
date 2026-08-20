using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.User.Create;

public record Command(string Name, string Email, string Password, bool? Active, string Role)
    : ICommand<Response>;