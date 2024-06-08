using FluentValidation.TestHelper;
using ProjectTemplate2024.Application.Game.Queries.GetUserGames;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Game.Queries.GetUserGames;

public class GetGameValidationTests
{
    private readonly GetUserGamesQueryValidation _validator = new();

    [Theory]
    [InlineData("test_one")]
    [InlineData("test_two")]
    [InlineData("test three")]
    [InlineData("test four")]
    [InlineData("test 5")]
    public void ValidInput_ThrowsNoErrors(string userId)
    {
        var result = this._validator.TestValidate(
            new GetUserGamesQuery(userId)
        );
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("   ")]
    [InlineData(null)]
    public void InvalidInput_Should_ThrowErrors(string userId)
    {
        var result = this._validator.TestValidate(
            new GetUserGamesQuery(userId)
        );
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}
