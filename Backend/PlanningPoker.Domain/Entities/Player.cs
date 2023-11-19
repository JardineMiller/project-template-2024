using PlanningPoker.Domain.Entities.Base;

namespace PlanningPoker.Domain.Entities;

public class Player : AuditableEntity
{
    public string? UserId { get; set; }
    public virtual User? User { get; set; }

    public string DisplayName { get; init; }

    public bool IsAnon => this.UserId is null;

    public virtual ICollection<Game> Games { get; set; } =
        new List<Game>();
}
