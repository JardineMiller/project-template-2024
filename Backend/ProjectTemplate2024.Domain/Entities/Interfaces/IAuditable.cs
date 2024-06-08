namespace ProjectTemplate2024.Domain.Entities.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? ModifiedOn { get; set; }
}
