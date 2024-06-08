using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Game.Queries.GetUserGames;

public record GetUserGamesQuery(string UserId)
    : IRequest<ErrorOr<GetUserGamesResult>>;
