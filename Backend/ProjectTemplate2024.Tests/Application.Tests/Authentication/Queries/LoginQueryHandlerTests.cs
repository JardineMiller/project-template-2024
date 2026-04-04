using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Authentication.Queries;

public class LoginQueryHandlerTests
{
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IBlobStorageService> _blobStorageServiceMock = new();

    private const string validDisplayName = "Test";
    private const string validEmail = "test1@user.com";
    private const string validPassword = "Password123!";

    private const string invalidEmail = "doesnt@exist.com";
    private const string invalidPassword = "IncorrectPassword!";

    public LoginQueryHandlerTests()
    {
        _tokenGeneratorMock = new Mock<ITokenGenerator>();
        _tokenGeneratorMock
            .Setup(x => x.GenerateJwt(It.IsAny<User>()))
            .Returns("token");

        _tokenGeneratorMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(new RefreshToken { Token = "refresh-token" });

        _userManagerMock = new Mock<UserManager<User>>(
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

        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(new DateTime(2023, 1, 1));
    }

    [Fact]
    public async Task Handle_GivenNonExistingUser_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.Users)
            .Returns(new List<User>().AsQueryable());

        _userRepositoryMock
            .Setup(
                x =>
                    x.GetUserByEmail(
                        invalidEmail,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()
                    )
            )
            .ReturnsAsync((User?)null);

        var query = new LoginQuery(invalidEmail, validPassword);
        var handler = new LoginQueryHandler(
            _tokenGeneratorMock.Object,
            _dateTimeProviderMock.Object,
            _userRepositoryMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
    public async Task Handle_GivenIncorrectPassword_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.Users)
            .Returns(
                new List<User>
                {
                    new User
                    {
                        DisplayName = validDisplayName,
                        Email = validEmail,
                        EmailConfirmed = true
                    }
                }.AsQueryable()
            );

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), invalidPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            _tokenGeneratorMock.Object,
            _dateTimeProviderMock.Object,
            _userRepositoryMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
    public async Task Handle_GivenUserHasNotConfirmedEmail_ReturnsError()
    {
        // Arrange
        var user = new User
        {
            DisplayName = validDisplayName,
            Email = validEmail,
        };

        _userRepositoryMock
            .Setup(
                x =>
                    x.GetUserByEmail(
                        validEmail,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()
                    )
            )
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), invalidPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            _tokenGeneratorMock.Object,
            _dateTimeProviderMock.Object,
            _userRepositoryMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(Errors.Authentication.EmailNotConfirmed.Code);
        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Authentication.EmailNotConfirmed.Description
            );
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        var user = new User
        {
            DisplayName = validDisplayName,
            Email = validEmail,
            EmailConfirmed = true
        };

        _userRepositoryMock
            .Setup(
                x =>
                    x.GetUserByEmail(
                        validEmail,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()
                    )
            )
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), validPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var query = new LoginQuery(validEmail, validPassword);
        var handler = new LoginQueryHandler(
            _tokenGeneratorMock.Object,
            _dateTimeProviderMock.Object,
            _userRepositoryMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.RefreshToken.ShouldBe("refresh-token");
        result.Value.User.DisplayName.ShouldBe(validDisplayName);
        result.Value.User.Email.ShouldBe(validEmail);
    }
}
