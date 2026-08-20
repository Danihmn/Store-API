using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.User.Authenticate;

public record Query(string Email, string Password) : IQuery<Response>;