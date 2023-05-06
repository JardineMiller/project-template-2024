namespace PlanningPoker.Application.Game.Queries.JoinGame;

public record JoinGameResult(
    string PlayerName,
    string PlayerId,
    string GameCode
);
