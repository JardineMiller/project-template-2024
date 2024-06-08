using ProjectTemplate2024.Application.Common.Interfaces.Services;

namespace ProjectTemplate2024.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.Now;
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
