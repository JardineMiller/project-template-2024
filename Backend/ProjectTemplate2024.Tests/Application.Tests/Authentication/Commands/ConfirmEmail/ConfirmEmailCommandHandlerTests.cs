using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Authentication.Commands.ConfirmEmail;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
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

    private const string ValidEmail = "test2@email.com";
    private const string ValidToken = "tokens-are-awesome";
    private readonly User _validUser = new()
    {
        DisplayName = "Test User",
        Email = ValidEmail,
    };

    public ConfirmEmailCommandHandlerTests()
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
    public async Task Handle_GivenNonExistingEmail_ReturnsInvalidCredError()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x =>
                x.GetUserByEmail(
                    ValidEmail,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<User, object>>[]>()
                )
            )
            .ReturnsAsync(null as User);

        var command = new ConfirmEmailCommand(ValidEmail, ValidToken);
        var handler = new ConfirmEmailCommandHandler(
            _userManagerMock.Object,
            _tokenGenerator.Object,
            _userRepositoryMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result
            .Errors.First()
            .Code.ShouldBe(Errors.Authentication.InvalidCredentials.Code);
        result
            .Errors.First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public async Task Handle_GivenInvalidEmailAndToken_ReturnsInvalidCredError()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x =>
                x.GetUserByEmail(ValidEmail, It.IsAny<CancellationToken>())
            )!
            .ReturnsAsync(_validUser);

        _userManagerMock
            .Setup(x =>
                x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())
            )
            .ReturnsAsync(IdentityResult.Failed());

        var command = new ConfirmEmailCommand(ValidEmail, ValidToken);
        var handler = new ConfirmEmailCommandHandler(
            _userManagerMock.Object,
            _tokenGenerator.Object,
            _userRepositoryMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result
            .Errors.First()
            .Code.ShouldBe(Errors.Authentication.InvalidCredentials.Code);
        result
            .Errors.First()
            .Description.ShouldBe(
                Errors.Authentication.InvalidCredentials.Description
            );
    }

    [Fact]
    public async Task Handle_GivenValidInput_ReturnsValidOutput()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x =>
                x.GetUserByEmail(
                    ValidEmail,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<User, object>>[]>()
                )
            )!
            .ReturnsAsync(_validUser);

        _userManagerMock
            .Setup(x =>
                x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())
            )
            .ReturnsAsync(IdentityResult.Success);

        _tokenGenerator
            .Setup(x => x.GenerateJwt(It.IsAny<User>()))
            .Returns("token");

        _tokenGenerator
            .Setup(x => x.GenerateRefreshToken())
            .Returns(new RefreshToken());

        var command = new ConfirmEmailCommand(ValidEmail, ValidToken);
        var handler = new ConfirmEmailCommandHandler(
            _userManagerMock.Object,
            _tokenGenerator.Object,
            _userRepositoryMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.User.DisplayName.ShouldBe(_validUser.DisplayName);
        result.Value.User.Email.ShouldBe(_validUser.Email);
    }
}
