using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Game.Queries.GetGame;

public record GetGameResult(
    string Name,
    string? Description,
    string Code,
    string OwnerId,
    User? Owner
);
