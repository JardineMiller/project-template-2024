using System;
using FluentValidation.TestHelper;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Application.Tests.TestHelpers;
using PlanningPoker.Domain.Common.Validation;
using Xunit;

namespace PlanningPoker.Application.Tests.Application.Tests.Game.Commands.Create;

public class CreateGameValidationTests
{
    private readonly CreateGameCommandValidation _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    public void Should_Have_Error_When_Name_Is_Invalid(
        string invalidName
    )
    {
        var command = new CreateGameCommand(
            invalidName,
            null,
            Guid.NewGuid().ToString()
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.OwnerId);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Big()
    {
        var invalidName = TestDataGenerator.GenerateRandomString(
            Validation.Game.Name.MaxLength + 1
        );

        var command = new CreateGameCommand(
            invalidName,
            null,
            Guid.NewGuid().ToString()
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.OwnerId);

        var validName = TestDataGenerator.GenerateRandomString(
            Validation.Game.Name.MaxLength + 1
        );

        var validCommand = new CreateGameCommand(
            validName,
            null,
            Guid.NewGuid().ToString()
        );

        var validResult = this._validator.TestValidate(validCommand);

        validResult.ShouldHaveValidationErrorFor(x => x.Name);
        validResult.ShouldNotHaveValidationErrorFor(
            x => x.Description
        );
        validResult.ShouldNotHaveValidationErrorFor(x => x.OwnerId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    public void Should_Have_Error_When_Description_Is_Invalid(
        string invalidDescription
    )
    {
        var command = new CreateGameCommand(
            invalidDescription,
            null,
            Guid.NewGuid().ToString()
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.OwnerId);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Big()
    {
        var invalidDescription =
            TestDataGenerator.GenerateRandomString(
                Validation.Game.Description.MaxLength + 1
            );

        var command = new CreateGameCommand(
            "Test Name",
            invalidDescription,
            Guid.NewGuid().ToString()
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.OwnerId);

        var validDescription = TestDataGenerator.GenerateRandomString(
            Validation.Game.Description.MaxLength + 1
        );

        var validCommand = new CreateGameCommand(
            "Test Name",
            validDescription,
            Guid.NewGuid().ToString()
        );

        var validResult = this._validator.TestValidate(validCommand);

        validResult.ShouldHaveValidationErrorFor(x => x.Description);
        validResult.ShouldNotHaveValidationErrorFor(x => x.Name);
        validResult.ShouldNotHaveValidationErrorFor(x => x.OwnerId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("not_a_guid")]
    [InlineData("asdbajbasd-212asdas-asdasda")]
    public void Should_Have_Error_When_OwnerId_Is_Invalid(
        string invalidOwnerId
    )
    {
        var command = new CreateGameCommand(
            "Test Name",
            null,
            invalidOwnerId
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.OwnerId);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
