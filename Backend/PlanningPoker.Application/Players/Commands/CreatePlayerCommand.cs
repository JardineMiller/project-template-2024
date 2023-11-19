using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Players.Commands;

public record CreatePlayerCommand(string UserId, string DisplayName)
    : IRequest<ErrorOr<CreatePlayerResult>>;
