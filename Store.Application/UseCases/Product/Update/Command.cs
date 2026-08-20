using Store.Application.Abstractions.Messaging;

namespace Store.Application.UseCases.Product.Update;

public sealed record Command (Guid Id, string Description, decimal UnitPrice, int? Stock) : ICommand<Response>;
