using Mapster;
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
    private const string validEmail = "email@email.com";
    private const string validPassword = "newPassword123!";
    private const string validOldPassword = "oldPassword123!";
    private const string validToken = "token";

    public AccountMappingConfigTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        AccountMappingConfig.AddConfig(config);
    }

    [Fact]
    public void ResetPasswordRequest_WithTokenProvided_ShouldMapTo_ResetPasswordCommand_WithNullOldPassword()
    {
        var src = new ResetPasswordRequest(
            validEmail,
            validPassword,
            validToken
        );

        var result = src.Adapt<ResetPasswordCommand>();

        result.Email.ShouldBe(validEmail);
        result.NewPassword.ShouldBe(validPassword);
        result.Token.ShouldBe(validToken);
        result.OldPassword.ShouldBe(null);
    }

    [Fact]
    public void ResetPasswordRequest_WithOldPasswordProvided_ShouldMapTo_ResetPasswordCommand_WithNullToken()
    {
        var src = new ResetPasswordRequest(
            validEmail,
            validPassword,
            null,
            validOldPassword
        );

        var result = src.Adapt<ResetPasswordCommand>();

        result.Email.ShouldBe(validEmail);
        result.NewPassword.ShouldBe(validPassword);
        result.Token.ShouldBe(null);
        result.OldPassword.ShouldBe(validOldPassword);
    }

    [Fact]
    public void ResetPasswordResult_ShouldMapTo_ResetPasswordResponse()
    {
        var src = new ResetPasswordResult();
        var result = src.Adapt<ResetPasswordResponse>();

        result.ShouldBeOfType<ResetPasswordResponse>();
    }

    [Fact]
    public void RequestResetPasswordRequest_ShouldMapTo_RequestResetPasswordCommand()
    {
        var src = new RequestResetPasswordRequest(validEmail);
        var result = src.Adapt<RequestResetPasswordCommand>();

        result.Email.ShouldBe(validEmail);
    }

    [Fact]
    public void RequestResetPasswordResult_ShouldMapTo_RequestResetPasswordResponse()
    {
        var src = new RequestResetPasswordResult(validToken);
        var result = src.Adapt<RequestResetPasswordResponse>();

        result.ShouldBeOfType<RequestResetPasswordResponse>();
        result.Token.ShouldBe(validToken);
    }
}
