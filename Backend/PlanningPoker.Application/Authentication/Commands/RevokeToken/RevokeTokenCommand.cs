using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Authentication.Commands.RevokeToken;

public record RevokeTokenCommand(string Token)
    : IRequest<ErrorOr<bool>>;
