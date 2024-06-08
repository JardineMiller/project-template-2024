using PlanningPoker.Domain.Entities.Interfaces;

namespace PlanningPoker.Domain.Entities.Base;

public abstract class AuditableEntity : BaseEntity, IAuditable
{
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}
