using ProjectTemplate2024.Domain.Entities.Interfaces;

namespace ProjectTemplate2024.Domain.Entities.Base;

public abstract class AuditableEntity : BaseEntity, IAuditable
{
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}
