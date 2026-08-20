using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Address.Create;

public sealed record Command (string Street, string City, string State, string ZipCode) : ICommand<Response>;
