namespace ProjectTemplate2024.Application.Common.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTimeOffset Now { get; }
}
