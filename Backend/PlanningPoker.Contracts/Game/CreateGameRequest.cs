namespace PlanningPoker.Contracts.Game;

public record CreateGameRequest(
    string Name,
    string? Description,
    string OwnerId
);
