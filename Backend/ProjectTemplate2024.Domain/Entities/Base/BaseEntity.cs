using ProjectTemplate2024.Domain.Entities.Interfaces;

namespace ProjectTemplate2024.Domain.Entities.Base;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}
