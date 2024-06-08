﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using PlanningPoker.Application.Authentication.Queries.Login;
using PlanningPoker.Application.Common.Interfaces.Authentication;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Application.Tests.Authentication.Queries;

public class LoginQueryHandlerTests
{
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private const string validFirstName = "Test";
    private const string validLastName = "User";
    private const string validEmail = "test1@user.com";
    private const string validPassword = "Password123!";

    private const string invalidEmail = "doesnt@exist.com";
    private const string invalidPassword = "IncorrectPassword!";

    public LoginQueryHandlerTests()
    {
        this._tokenGeneratorMock = new Mock<ITokenGenerator>();
        this._tokenGeneratorMock
            .Setup(x => x.GenerateJwt(It.IsAny<User>()))
            .Returns("token");

        this._tokenGeneratorMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(new RefreshToken { Token = "refresh-token" });

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

        this._dateTimeProviderMock = new Mock<IDateTimeProvider>();
        this._dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(new DateTime(2023, 1, 1));
    }

    [Fact]
    public async Task Handle_GivenNonExistingUser_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.Users)
            .Returns(new List<User>().AsQueryable());

        var query = new LoginQuery(invalidEmail, validPassword);
        var handler = new LoginQueryHandler(
            this._tokenGeneratorMock.Object,
            this._userManagerMock.Object,
            this._dateTimeProviderMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
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
    public async Task Handle_GivenIncorrectPassword_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.Users)
            .Returns(
                new List<User>
                {
                    new User
                    {
                        FirstName = validFirstName,
                        LastName = validLastName,
                        Email = validEmail,
                        EmailConfirmed = true
                    }
                }.AsQueryable()
            );

        this._userManagerMock
            .Setup(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<User>(),
                        invalidPassword
                    )
            )!
            .ReturnsAsync(false);

        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            this._tokenGeneratorMock.Object,
            this._userManagerMock.Object,
            this._dateTimeProviderMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
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
    public async Task Handle_GivenUserHasNotConfirmedEmail_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.Users)
            .Returns(
                new List<User>
                {
                    new User
                    {
                        FirstName = validFirstName,
                        LastName = validLastName,
                        Email = validEmail,
                    }
                }.AsQueryable()
            );

        this._userManagerMock
            .Setup(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<User>(),
                        invalidPassword
                    )
            )!
            .ReturnsAsync(false);

        var query = new LoginQuery(validEmail, invalidPassword);
        var handler = new LoginQueryHandler(
            this._tokenGeneratorMock.Object,
            this._userManagerMock.Object,
            this._dateTimeProviderMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(
                Errors.Authentication.EmailNotConfirmed.Code
            );
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
        this._userManagerMock
            .Setup(x => x.Users)
            .Returns(
                new List<User>
                {
                    new User
                    {
                        FirstName = validFirstName,
                        LastName = validLastName,
                        Email = validEmail,
                        EmailConfirmed = true
                    }
                }.AsQueryable()
            );

        this._userManagerMock
            .Setup(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<User>(),
                        validPassword
                    )
            )!
            .ReturnsAsync(true);

        var query = new LoginQuery(validEmail, validPassword);
        var handler = new LoginQueryHandler(
            this._tokenGeneratorMock.Object,
            this._userManagerMock.Object,
            this._dateTimeProviderMock.Object
        );

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.Value.Token.ShouldBe("token");
        result.Value.RefreshToken.ShouldBe("refresh-token");
        result.Value.User.FirstName.ShouldBe(validFirstName);
        result.Value.User.LastName.ShouldBe(validLastName);
        result.Value.User.Email.ShouldBe(validEmail);
    }
}
