namespace ProjectTemplate2024.Contracts.Game.GetUserGames;

public record GetUserGamesResponse(
    string UserId,
    ICollection<Domain.Entities.Game> Games //TODO: Stub?
);
