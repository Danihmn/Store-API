using FluentResults;
using MediatR;

namespace Store.Application.UseCases.User.Create;

public record Command(string Name, string Email, string Password, bool? Active, string Role)
    : IRequest<Result<Response>>;