using ProjectTemplate2024.Api.Common.Mapping;
using ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using ProjectTemplate2024.Contracts.Account.RequestResetPassword;
using ProjectTemplate2024.Contracts.Account.ResetPassword;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Api.Tests.Common.Mapping;

public class AccountMappingConfigTests
{
    private const string ValidEmail = "email@email.com";
    private const string ValidPassword = "newPassword123!";
    private const string ValidOldPassword = "oldPassword123!";
    private const string ValidToken = "token";

    public AccountMappingConfigTests() { }

    [Fact]
    public void ResetPasswordRequest_WithTokenProvided_ShouldMapTo_ResetPasswordCommand_WithNullOldPassword()
    {
        var src = new ResetPasswordRequest(
            ValidEmail,
            ValidPassword,
            ValidToken
        );

        var result = src.ToCommand();

        result.Email.ShouldBe(ValidEmail);
        result.NewPassword.ShouldBe(ValidPassword);
        result.Token.ShouldBe(ValidToken);
        result.OldPassword.ShouldBe(null);
    }

    [Fact]
    public void ResetPasswordRequest_WithOldPasswordProvided_ShouldMapTo_ResetPasswordCommand_WithNullToken()
    {
        var src = new ResetPasswordRequest(
            ValidEmail,
            ValidPassword,
            null,
            ValidOldPassword
        );

        var result = src.ToCommand();

        result.Email.ShouldBe(ValidEmail);
        result.NewPassword.ShouldBe(ValidPassword);
        result.Token.ShouldBe(null);
        result.OldPassword.ShouldBe(ValidOldPassword);
    }

    [Fact]
    public void ResetPasswordResult_ShouldMapTo_ResetPasswordResponse()
    {
        var src = new ResetPasswordResult();
        var result = src.ToResponse();

        result.ShouldBeOfType<ResetPasswordResponse>();
    }

    [Fact]
    public void RequestResetPasswordRequest_ShouldMapTo_RequestResetPasswordCommand()
    {
        var src = new RequestResetPasswordRequest(ValidEmail);
        var result = src.ToCommand();

        result.Email.ShouldBe(ValidEmail);
    }

    [Fact]
    public void RequestResetPasswordResult_ShouldMapTo_RequestResetPasswordResponse()
    {
        var src = new RequestResetPasswordResult(ValidToken);
        var result = src.ToResponse();

        result.ShouldBeOfType<RequestResetPasswordResponse>();
        result.Token.ShouldBe(ValidToken);
    }
}
