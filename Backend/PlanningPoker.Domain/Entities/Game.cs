using PlanningPoker.Domain.Entities.Base;

namespace PlanningPoker.Domain.Entities;

public class Game : DeletableEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Code { get; set; }

    public string OwnerId { get; set; }
    public User Owner { get; set; }
}
