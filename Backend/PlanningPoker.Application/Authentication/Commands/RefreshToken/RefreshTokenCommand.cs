using ErrorOr;
using MediatR;
using PlanningPoker.Application.Authentication.Common;

namespace PlanningPoker.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string Token)
    : IRequest<ErrorOr<AuthenticationResult>>;
