using FluentValidation.TestHelper;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.ResetPassword;

public class ResetPasswordValidationTests
{
    private readonly ResetPasswordCommandValidation _validator;

    private const string ValidPassword = "Password123!";
    private const string ValidEmail = "test@user.com";
    private const string ValidToken = "token";

    public ResetPasswordValidationTests()
    {
        _validator = new ResetPasswordCommandValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("just_a_random_string")]
    private void Should_Have_Error_When_Email_Is_Invalid(string? invalidEmail)
    {
        var query = new ResetPasswordCommand(
            invalidEmail!,
            ValidPassword,
            ValidToken
        );
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
        result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
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
        var command = new ResetPasswordCommand(
            ValidEmail,
            invalidPassword!,
            ValidToken
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
        result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    public void Should_Have_Error_When_Token_Is_Invalid(string? invalidToken)
    {
        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidPassword,
            invalidToken
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_Token_And_OldPassword_Not_Provided()
    {
        var command = new ResetPasswordCommand(ValidEmail, ValidPassword);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
        result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_OldPassword_And_NewPassword_Are_Equals()
    {
        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidPassword,
            null,
            ValidPassword
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
    }

    [Fact]
    public void Should_Have_Error_When_OldPassword_And_Token_Provided()
    {
        var command = new ResetPasswordCommand(
            ValidEmail,
            "newPassword123!",
            ValidToken,
            ValidPassword
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
        result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
