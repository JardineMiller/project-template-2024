using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Game.Queries.GetGame;

public record GetGameQuery(string Code)
    : IRequest<ErrorOr<GetGameResult>>;
