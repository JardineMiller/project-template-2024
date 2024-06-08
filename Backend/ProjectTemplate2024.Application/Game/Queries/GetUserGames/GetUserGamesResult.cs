namespace ProjectTemplate2024.Application.Game.Queries.GetUserGames;

public record GetUserGamesResult(
    string UserId,
    ICollection<Domain.Entities.Game> Games
);
