using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Game.Queries;

public record GetGameQuery(string Code)
    : IRequest<ErrorOr<GetGameResult>>;
