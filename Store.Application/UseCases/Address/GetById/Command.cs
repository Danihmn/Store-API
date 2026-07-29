using FluentResults;
using MediatR;

namespace Store.Application.UseCases.Address.GetById;

public sealed record Command(Guid Id) : IRequest<Result<Response>>;