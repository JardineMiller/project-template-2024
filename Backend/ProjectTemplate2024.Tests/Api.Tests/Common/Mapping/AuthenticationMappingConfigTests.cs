using Mapster;
using Shouldly;
using ProjectTemplate2024.Api.Common.Mapping;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using ProjectTemplate2024.Contracts.Authentication;
using ProjectTemplate2024.Domain.Entities;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Api.Tests.Common.Mapping;

public class AuthenticationMappingConfigTests
{
    public AuthenticationMappingConfigTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        AuthenticationMappingConfig.AddConfig(config);
    }

    [Fact]
    public void RegisterRequest_ShouldMapTo_RegisterCommand()
    {
        var src = new RegisterRequest(
            "FirstName",
            "LastName",
            "Email",
            "Password"
        );

        var result = src.Adapt<RegisterCommand>();

        result.FirstName.ShouldBe(src.FirstName);
        result.LastName.ShouldBe(src.LastName);
        result.Email.ShouldBe(src.Email);
        result.Password.ShouldBe(src.Password);
    }

    [Fact]
    public void LoginRequest_ShouldMapTo_LoginQuery()
    {
        var src = new LoginRequest("Email", "Password");

        var result = src.Adapt<LoginQuery>();

        result.Email.ShouldBe(src.Email);
        result.Password.ShouldBe(src.Password);
    }

    [Fact]
    public void AuthenticationResult_ShouldMapTo_AuthenticationResponse_WithNullToken()
    {
        var user = new User()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "Email",
        };

        var src = new AuthenticationResult(user);

        var result = src.Adapt<AuthenticationResponse>();

        result.FirstName.ShouldBe(user.FirstName);
        result.LastName.ShouldBe(user.LastName);
        result.Email.ShouldBe(user.Email);

        result.Token.ShouldBe(null);
    }

    [Fact]
    public void AuthenticationResult_ShouldMapTo_AuthenticationResponse_WithoutNullToken()
    {
        var user = new User()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "Email",
        };

        var src = new AuthenticationResult(user, "token");

        var result = src.Adapt<AuthenticationResponse>();

        result.FirstName.ShouldBe(user.FirstName);
        result.LastName.ShouldBe(user.LastName);
        result.Email.ShouldBe(user.Email);

        result.Token.ShouldBe("token");
    }
}
