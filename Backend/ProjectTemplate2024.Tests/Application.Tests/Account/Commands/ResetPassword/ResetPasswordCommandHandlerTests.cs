using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    private const string ValidEmail = "test2@email.com";
    private const string ValidOldPassword = "newPassword123!";
    private const string ValidNewPassword = "oldPassword123!";
    private const string ValidToken = "validToken";

    public ResetPasswordCommandHandlerTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!
        );
    }

    [Fact]
    public async Task Handle_ValidOldPasswordRequest_ReturnsCorrectResponse()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(new User { Email = ValidEmail });

        _userManagerMock
            .Setup(
                x =>
                    x.ChangePasswordAsync(
                            It.IsAny<User>(),
                            ValidOldPassword,
                            ValidNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Success);

        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidNewPassword,
            null,
            ValidOldPassword
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBe(false);
        result.Value.ShouldBeOfType<ResetPasswordResult>();
    }

    [Fact]
    public async Task Handle_InvalidOldPasswordRequest_IncorrectEmail_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(null as User);

        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidNewPassword,
            null,
            ValidOldPassword
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

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
    public async Task Handle_InvalidOldPasswordRequest_IncorrectToken_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(new User { Email = ValidEmail });

        _userManagerMock
            .Setup(
                x =>
                    x.ChangePasswordAsync(
                        It.IsAny<User>(),
                        ValidOldPassword,
                        ValidNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidNewPassword,
            null,
            ValidOldPassword
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

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
    public async Task Handle_ValidTokenRequest_ReturnsCorrectResponse()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(new User { Email = ValidEmail });

        _userManagerMock
            .Setup(
                x =>
                    x.ResetPasswordAsync(
                        It.IsAny<User>(),
                        ValidToken,
                        ValidNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Success);

        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidNewPassword,
            ValidToken
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBe(false);
        result.Value.ShouldBeOfType<ResetPasswordResult>();
    }

    [Fact]
    public async Task Handle_InvalidTokenRequest_IncorrectEmail_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(null as User);

        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidNewPassword,
            ValidToken
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

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
    public async Task Handle_InvalidTokenRequest_IncorrectToken_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(new User { Email = ValidEmail });

        _userManagerMock
            .Setup(
                x =>
                    x.ResetPasswordAsync(
                        It.IsAny<User>(),
                        ValidToken,
                        ValidNewPassword
                    )
            )
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ResetPasswordCommand(
            ValidEmail,
            ValidNewPassword,
            ValidToken
        );

        // Act
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

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
