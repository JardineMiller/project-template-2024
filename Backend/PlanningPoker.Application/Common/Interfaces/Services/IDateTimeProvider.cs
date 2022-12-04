namespace PlanningPoker.Application.Common.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTimeOffset Now { get; }
}
