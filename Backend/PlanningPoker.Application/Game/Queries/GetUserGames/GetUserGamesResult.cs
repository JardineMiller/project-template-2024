namespace PlanningPoker.Application.Game.Queries.GetUserGames;

public record GetUserGamesResult(
    string UserId,
    ICollection<Domain.Entities.Game> Games
);
