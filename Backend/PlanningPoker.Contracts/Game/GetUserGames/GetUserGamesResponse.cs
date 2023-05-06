namespace PlanningPoker.Contracts.Game.GetUserGames;

public record GetUserGamesResponse(
    string UserId,
    ICollection<Domain.Entities.Game> Games //TODO: Stub?
);
