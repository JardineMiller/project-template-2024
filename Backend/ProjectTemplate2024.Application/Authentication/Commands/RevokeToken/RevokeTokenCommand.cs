using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Authentication.Commands.RevokeToken;

public record RevokeTokenCommand(string Token)
    : IRequest<ErrorOr<bool>>;
