using ProjectTemplate2024.Infrastructure.Generators;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Infrastructure.Tests.Generators;

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
