using Mapster;
using ProjectTemplate2024.Api.Common.Mapping;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Api.Tests.Common.Mapping;

public class DefaultMappingConfigTests
{
    public class TestClass
    {
        public string TestProperty { get; set; }

        public TestClass(string testProperty)
        {
            this.TestProperty = testProperty;
        }
    }
    
    [Fact]
    public void Should_Trim_Whitespace_Properties()
    {
        var config = new TypeAdapterConfig();
        DefaultMappingConfig.AddConfig(config);
        
        var testClass = new TestClass(
            "   with spaces   "
        );

        var result = testClass.Adapt<TestClass>(config);

        result.TestProperty.ShouldBe("with spaces");
    }
}
