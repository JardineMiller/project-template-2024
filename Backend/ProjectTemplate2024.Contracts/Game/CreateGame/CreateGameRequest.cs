namespace ProjectTemplate2024.Contracts.Game.CreateGame;

public record CreateGameRequest(
    string Name,
    string? Description,
    string OwnerId
);
