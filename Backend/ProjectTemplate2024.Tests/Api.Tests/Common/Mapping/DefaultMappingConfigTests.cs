using ProjectTemplate2024.Api.Common.Mapping;
using ProjectTemplate2024.Contracts.Authentication;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Api.Tests.Common.Mapping;

public class DefaultMappingConfigTests
{
    [Fact]
    public void Should_Trim_Whitespace_In_Request_To_Command_Mapping()
    {
        var src = new RegisterRequest(
            "   with spaces   ",
            "  email@x.com  ",
            "  pwd  "
        );

        var result = src.ToCommand();

        result.DisplayName.ShouldBe("with spaces");
        result.Email.ShouldBe("email@x.com");
        result.Password.ShouldBe("pwd");
    }
}
