using PlanningPoker.Infrastructure.Generators;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Infrastructure.Tests.Generators;

public class TinyGuidGeneratorTests
{
    [Fact]
    public void Generate_GeneratesTinyGuid()
    {
        var generator = new TinyGuidGenerator();

        var result = generator.Generate();

        result.ShouldNotBeNull();
        result.Length.ShouldBe(8);
    }
}
