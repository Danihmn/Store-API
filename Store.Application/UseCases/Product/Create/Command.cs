using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Product.Create;

public sealed record Command (string Description, decimal UnitPrice, int? Stock) : ICommand<Response>;
