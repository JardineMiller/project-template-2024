using FluentValidation.TestHelper;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.UpdateUser;

public class UpdateUserCommandValidationTests
{
    private readonly UpdateUserCommandValidation _validator;

    public UpdateUserCommandValidationTests()
    {
        _validator = new UpdateUserCommandValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("just_a_random_string")]
    public void Should_Have_Error_When_Email_Is_Invalid(string? invalidEmail)
    {
        var command = new UpdateUserCommand
        {
            Email = invalidEmail!,
            DisplayName = "Valid Name"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.DisplayName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // 51 chars
    public void Should_Have_Error_When_DisplayName_Is_Invalid(string? invalidDisplayName)
    {
        var command = new UpdateUserCommand
        {
            Email = "valid@user.com",
            DisplayName = invalidDisplayName!
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.DisplayName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Bio_Is_Too_Long()
    {
        var longBio = new string('a', 251);
        var command = new UpdateUserCommand
        {
            Email = "valid@user.com",
            DisplayName = "Valid",
            Bio = longBio
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Bio);
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData(" ")]
    [InlineData("http:/bad")]
    public void Should_Have_Error_When_AvatarUrl_Is_Invalid(string invalidUrl)
    {
        var command = new UpdateUserCommand
        {
            Email = "valid@user.com",
            DisplayName = "Valid",
            AvatarUrl = invalidUrl
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AvatarUrl);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("https://example.com/avatar.png")]
    public void Should_Not_Have_Error_When_AvatarUrl_Is_Valid(string? url)
    {
        var command = new UpdateUserCommand
        {
            Email = "valid@user.com",
            DisplayName = "Valid",
            AvatarUrl = url
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.AvatarUrl);
    }

    [Fact]
    public void Should_Not_Have_Error_With_Valid_Input()
    {
        var command = new UpdateUserCommand
        {
            Email = "test@user.com",
            DisplayName = "Valid User",
            Bio = "Short bio",
            AvatarUrl = null
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.DisplayName);
        result.ShouldNotHaveValidationErrorFor(x => x.Bio);
        result.ShouldNotHaveValidationErrorFor(x => x.AvatarUrl);
    }
}
