using ProjectTemplate2024.Application.Common.Interfaces.Generators;

namespace ProjectTemplate2024.Infrastructure.Generators;

public class TinyGuidGenerator : ITinyGuidGenerator
{
    private static readonly Random random = new();

    public string Generate()
    {
        return random.Next(int.MaxValue).ToString("x8");
    }
}
