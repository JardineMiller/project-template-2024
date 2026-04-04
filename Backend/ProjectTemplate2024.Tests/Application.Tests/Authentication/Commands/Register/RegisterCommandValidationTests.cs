using FluentValidation.TestHelper;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Authentication.Commands.Register;

public class RegisterCommandValidationTests
{
    private readonly RegisterCommandValidation _validator;
    private const string ValidPassword = "Password123!";
    private const string ValidDisplayName = "Test User";
    private const string ValidEmail = "test@user.com";

    public RegisterCommandValidationTests()
    {
        _validator = new RegisterCommandValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
    )]
    public void Should_Have_Error_When_FirstName_Is_Invalid(
        string? invalidDisplayName
    )
    {
        var command = new RegisterCommand(
            invalidDisplayName!,
            ValidEmail,
            ValidPassword
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.DisplayName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
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
        var command = new RegisterCommand(
            ValidDisplayName,
            invalidEmail!,
            ValidPassword
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.DisplayName);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("p")]
    [InlineData("password")]
    [InlineData("password1")]
    [InlineData("password!")]
    [InlineData("Password")]
    [InlineData("Password1")]
    [InlineData("Password!")]
    [InlineData("More_Than_Sixteen_Digits_Long_1!")]
    public void Should_Have_Error_When_Password_Is_Invalid(
        string? invalidPassword
    )
    {
        var command = new RegisterCommand(
            ValidDisplayName,
            ValidEmail,
            invalidPassword!
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.DisplayName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_With_Valid_Input()
    {
        var command = new RegisterCommand(
            ValidDisplayName,
            ValidEmail,
            ValidPassword
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.DisplayName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
