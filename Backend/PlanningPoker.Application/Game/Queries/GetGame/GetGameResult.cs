using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Game.Queries.GetGame;

public record GetGameResult(
    string Name,
    string? Description,
    string Code,
    string OwnerId,
    User? Owner
)
{
    public static class From
    {
        public static GetGameResult Game(Domain.Entities.Game game)
        {
            return new GetGameResult(
                game.Name,
                game.Description,
                game.Code,
                game.OwnerId,
                game.Owner
            );
        }
    }
}
