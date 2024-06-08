using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Contracts.Game.GetGame;

public record GetGameResponse(
    string Name,
    string Description,
    string Code,
    string OwnerId,
    User? Owner
);
