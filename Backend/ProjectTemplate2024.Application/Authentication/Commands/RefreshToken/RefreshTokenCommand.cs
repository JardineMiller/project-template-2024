using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Authentication.Common;

namespace ProjectTemplate2024.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string Token)
    : IRequest<ErrorOr<AuthenticationResult>>;
