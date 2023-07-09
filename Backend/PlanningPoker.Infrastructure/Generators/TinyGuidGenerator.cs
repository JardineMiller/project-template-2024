using PlanningPoker.Application.Common.Interfaces.Generators;

namespace PlanningPoker.Infrastructure.Generators;

public class TinyGuidGenerator : ITinyGuidGenerator
{
    private static readonly Random random = new();

    public string Generate()
    {
        return random.Next(int.MaxValue).ToString("x8");
    }
}
