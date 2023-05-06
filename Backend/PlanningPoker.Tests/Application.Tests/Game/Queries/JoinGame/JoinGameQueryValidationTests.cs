using FluentValidation.TestHelper;
using PlanningPoker.Application.Game.Queries.JoinGame;
using Xunit;

namespace PlanningPoker.Application.Tests.Application.Tests.Game.Queries.JoinGame;

public class JoinGameQueryValidationTests
{
    private readonly JoinGameQueryValidation _validator = new();

    [Theory]
    [InlineData("code-1", "user-id")]
    [InlineData("code", "user-id")]
    [InlineData("code-123", "user-id")]
    [InlineData("code-123", "s")]
    public void ValidInput_ThrowsNoErrors(
        string code,
        string playerId
    )
    {
        var result = this._validator.TestValidate(
            new JoinGameQuery(code, playerId)
        );

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    public void InvalidInput_Should_ThrowErrors(
        string code,
        string playerId
    )
    {
        var result = this._validator.TestValidate(
            new JoinGameQuery(code, playerId)
        );

        result.ShouldHaveValidationErrorFor(x => x.GameCode);
        result.ShouldHaveValidationErrorFor(x => x.PlayerId);
    }
}
