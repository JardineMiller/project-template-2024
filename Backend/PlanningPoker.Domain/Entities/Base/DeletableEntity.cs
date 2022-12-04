using PlanningPoker.Domain.Entities.Interfaces;

namespace PlanningPoker.Domain.Entities.Base;

public abstract class DeletableEntity : AuditableEntity, IDeletable
{
    public DateTimeOffset? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}
