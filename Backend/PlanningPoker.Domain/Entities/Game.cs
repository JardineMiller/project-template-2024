using PlanningPoker.Domain.Entities.Base;

namespace PlanningPoker.Domain.Entities;

public class Game : DeletableEntity
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public string Code { get; init; }

    public string OwnerId { get; init; }
    public User Owner { get; }
}
