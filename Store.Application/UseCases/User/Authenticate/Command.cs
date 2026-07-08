using FluentResults;
using MediatR;

namespace Store.Application.UseCases.User.Authenticate;

public record Command (string Email, string Password) : IRequest<Result<Response>>;