using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Game.Queries.JoinGame;

public record JoinGameQuery(string GameCode, string PlayerId)
    : IRequest<ErrorOr<JoinGameResult>>;
