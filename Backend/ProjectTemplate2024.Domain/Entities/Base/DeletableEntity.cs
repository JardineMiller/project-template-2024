using ProjectTemplate2024.Domain.Entities.Interfaces;

namespace ProjectTemplate2024.Domain.Entities.Base;

public abstract class DeletableEntity : AuditableEntity, IDeletable
{
    public DateTimeOffset? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}
