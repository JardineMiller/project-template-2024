using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Contracts.Game;

public record GetGameResponse(
    string Name,
    string Description,
    string Code,
    string OwnerId,
    User? Owner
);
