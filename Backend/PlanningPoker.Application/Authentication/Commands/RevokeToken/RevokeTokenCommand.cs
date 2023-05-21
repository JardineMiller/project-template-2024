using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Authentication.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest<ErrorOr<bool>>
{
    public string Token { get; set; }
}
