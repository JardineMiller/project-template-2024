using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.ResetPassword;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;

    private const string validEmail = "test2@email.com";
    private const string validOldPassword = "newPassword123!";
    private const string validNewPassword = "oldPassword123!";
    private const string validToken = "validToken";

    public ResetPasswordCommandHandlerTests()
    {
        this._userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
    }

    [Fact]
    public void Handle_ValidOldPasswordRequest_ReturnsCorrectResponse()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(new User { Email = validEmail });

        this._userManagerMock
            .Setup(
                x =>
                    x.ChangePasswordAsync(
                        It.IsAny<User>(),
                        validOldPassword,
                        validNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Success);

        var command = new ResetPasswordCommand(
            validEmail,
            validNewPassword,
            null,
            validOldPassword
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(false);
        result.Value.ShouldBeOfType<ResetPasswordResult>();
    }

    [Fact]
    public void Handle_InvalidOldPasswordRequest_IncorrectEmail_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(null as User);

        var command = new ResetPasswordCommand(
            validEmail,
            validNewPassword,
            null,
            validOldPassword
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(true);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.InvalidCredentials.Code
            );

        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_InvalidOldPasswordRequest_IncorrectToken_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(new User { Email = validEmail });

        this._userManagerMock
            .Setup(
                x =>
                    x.ChangePasswordAsync(
                        It.IsAny<User>(),
                        validOldPassword,
                        validNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ResetPasswordCommand(
            validEmail,
            validNewPassword,
            null,
            validOldPassword
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(true);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.InvalidCredentials.Code
            );

        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_ValidTokenRequest_ReturnsCorrectResponse()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(new User { Email = validEmail });

        this._userManagerMock
            .Setup(
                x =>
                    x.ResetPasswordAsync(
                        It.IsAny<User>(),
                        validToken,
                        validNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Success);

        var command = new ResetPasswordCommand(
            validEmail,
            validNewPassword,
            validToken
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(false);
        result.Value.ShouldBeOfType<ResetPasswordResult>();
    }

    [Fact]
    public void Handle_InvalidTokenRequest_IncorrectEmail_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(null as User);

        var command = new ResetPasswordCommand(
            validEmail,
            validNewPassword,
            validToken
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(true);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.InvalidCredentials.Code
            );

        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_InvalidTokenRequest_IncorrectToken_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(new User { Email = validEmail });

        this._userManagerMock
            .Setup(
                x =>
                    x.ResetPasswordAsync(
                        It.IsAny<User>(),
                        validToken,
                        validNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ResetPasswordCommand(
            validEmail,
            validNewPassword,
            validToken
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            this._userManagerMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(true);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.InvalidCredentials.Code
            );

        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }
}
