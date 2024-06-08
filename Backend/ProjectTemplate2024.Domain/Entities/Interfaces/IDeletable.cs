namespace ProjectTemplate2024.Domain.Entities.Interfaces;

public interface IDeletable
{
    public DateTimeOffset? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}
