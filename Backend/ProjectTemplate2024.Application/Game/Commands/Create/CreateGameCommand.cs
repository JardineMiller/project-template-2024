using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Game.Commands.Create;

public record CreateGameCommand(
    string Name,
    string? Description,
    string OwnerId
) : IRequest<ErrorOr<CreateGameResult>>;
