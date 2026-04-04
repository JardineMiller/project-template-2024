using FluentValidation.TestHelper;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Authentication.Queries;

public class LoginQueryValidationTests
{
    private readonly LoginQueryValidation _validator;

    private const string ValidPassword = "Password123!";
    private const string ValidEmail = "test@user.com";

    public LoginQueryValidationTests()
    {
        _validator = new LoginQueryValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("just_a_random_string")]
    private void Should_Have_Error_When_Email_Is_Invalid(
        string? invalidEmail
    )
    {
        var query = new LoginQuery(invalidEmail!, ValidPassword);
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    private void Should_Have_Error_When_Password_Is_Invalid(
        string? invalidPassword
    )
    {
        var query = new LoginQuery(ValidEmail, invalidPassword!);
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    private void Should_Pass_When_Valid_Input_Provided()
    {
        var query = new LoginQuery(ValidEmail, ValidPassword);
        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
