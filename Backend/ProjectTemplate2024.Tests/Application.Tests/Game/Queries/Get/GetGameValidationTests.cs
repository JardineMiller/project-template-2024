using FluentValidation.TestHelper;
using ProjectTemplate2024.Application.Game.Queries.GetGame;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Game.Queries.Get;

public class GetGameValidationTests
{
    private readonly GetGameQueryValidation _validator = new();

    [Theory]
    [InlineData("test_one")]
    [InlineData("test_two")]
    [InlineData("test three")]
    [InlineData("test four")]
    [InlineData("test 5")]
    public void ValidInput_ThrowsNoErrors(string code)
    {
        var result = this._validator.TestValidate(new GetGameQuery(code));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("   ")]
    [InlineData(null)]
    public void InvalidInput_Should_ThrowErrors(string code)
    {
        var result = this._validator.TestValidate(new GetGameQuery(code));
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }
}
