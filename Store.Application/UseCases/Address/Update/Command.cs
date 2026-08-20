using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Address.Update;

public sealed record Command (Guid Id, string Street, string City, string State, string ZipCode) : ICommand<Response>;
