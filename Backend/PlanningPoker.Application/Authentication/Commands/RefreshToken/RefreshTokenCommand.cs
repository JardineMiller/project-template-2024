using ErrorOr;
using MediatR;
using PlanningPoker.Application.Authentication.Common;

namespace PlanningPoker.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand
    : IRequest<ErrorOr<AuthenticationResult>>
{
    public string Token { get; set; }
}
