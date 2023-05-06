using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Players.Queries.GetPlayer;

public record GetPlayerQuery(string PlayerId)
    : IRequest<ErrorOr<GetPlayerResult>>;
