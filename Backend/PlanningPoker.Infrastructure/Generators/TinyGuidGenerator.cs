namespace PlanningPoker.Infrastructure.Generators;

public class TinyGuidGenerator
{
    private static readonly Random _random = new();

    public string Generate()
    {
        return _random.Next(int.MaxValue).ToString("x");
    }
}
