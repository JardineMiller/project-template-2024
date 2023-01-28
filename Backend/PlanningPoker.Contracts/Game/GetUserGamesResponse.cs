namespace PlanningPoker.Contracts.Game;

public record GetUserGamesResponse(
    string UserId,
    ICollection<Domain.Entities.Game> Games //TODO: Stub?
);
