using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Authentication.Commands.ConfirmEmail;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenGenerator> _tokenGenerator = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private const string validEmail = "test2@email.com";
    private const string validToken = "tokens-are-awesome";
    private readonly User _validUser =
        new()
        {
            FirstName = "Test",
            LastName = "User",
            Email = validEmail
        };

    public ConfirmEmailCommandHandlerTests()
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
    public void Handle_GivenNonExistingEmail_ReturnsInvalidCredError()
    {
        // Arrange
        this._userRepositoryMock
            .Setup(
                x =>
                    x.GetUserByEmail(
                        validEmail,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()
                    )
            )!
            .ReturnsAsync(null as User);

        var command = new ConfirmEmailCommand(validEmail, validToken);
        var handler = new ConfirmEmailCommandHandler(
            this._userManagerMock.Object,
            this._tokenGenerator.Object,
            this._userRepositoryMock.Object
        );

        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(Errors.Authentication.InvalidCredentials.Code);
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_GivenInvalidEmailAndToken_ReturnsInvalidCredError()
    {
        // Arrange
        this._userRepositoryMock
            .Setup(
                x => x.GetUserByEmail(validEmail, It.IsAny<CancellationToken>())
            )!
            .ReturnsAsync(this._validUser);

        this._userManagerMock
            .Setup(
                x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())
            )!
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ConfirmEmailCommand(validEmail, validToken);
        var handler = new ConfirmEmailCommandHandler(
            this._userManagerMock.Object,
            this._tokenGenerator.Object,
            this._userRepositoryMock.Object
        );

        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(Errors.Authentication.InvalidCredentials.Code);
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public void Handle_GivenValidInput_ReturnsValidOutput()
    {
        // Arrange
        this._userRepositoryMock
            .Setup(
                x =>
                    x.GetUserByEmail(
                        validEmail,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()
                    )
            )!
            .ReturnsAsync(this._validUser);

        this._userManagerMock
            .Setup(
                x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())
            )!
            .ReturnsAsync(IdentityResult.Success);

        this._tokenGenerator
            .Setup(x => x.GenerateJwt(It.IsAny<User>()))
            .Returns("token");

        this._tokenGenerator
            .Setup(x => x.GenerateRefreshToken())
            .Returns(new RefreshToken());

        var command = new ConfirmEmailCommand(validEmail, validToken);
        var handler = new ConfirmEmailCommandHandler(
            this._userManagerMock.Object,
            this._tokenGenerator.Object,
            this._userRepositoryMock.Object
        );

        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.User.FirstName.ShouldBe(this._validUser.FirstName);
        result.Value.User.LastName.ShouldBe(this._validUser.LastName);
        result.Value.User.Email.ShouldBe(this._validUser.Email);
    }
}
