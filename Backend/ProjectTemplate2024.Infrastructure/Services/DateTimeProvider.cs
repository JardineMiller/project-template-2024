using PlanningPoker.Application.Common.Interfaces.Services;

namespace PlanningPoker.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.Now;
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
