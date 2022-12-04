using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Moq;
using PlanningPoker.Application.Account.Commands.RequestResetPassword;
using PlanningPoker.Application.Account.Common;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Application.Tests.Account.Commands.RequestResetPassword;

public class RequestResetPasswordCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock;

    private const string validEmail = "test2@email.com";
    private const string validToken = "validToken";

    public RequestResetPasswordCommandHandlerTests()
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

        this._emailServiceMock = new Mock<IEmailService>();
    }

    [Fact]
    public void Handle_ValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(new User() { Email = validEmail });

        this._userManagerMock
            .Setup(
                x =>
                    x.GeneratePasswordResetTokenAsync(
                        It.IsAny<User>()
                    )
            )
            .ReturnsAsync(validToken);

        var command = new RequestResetPasswordCommand(validEmail);

        // Act
        var handler = new RequestResetPasswordCommandHandler(
            this._userManagerMock.Object,
            this._emailServiceMock.Object
        );

        var result = handler
            .Handle(command, CancellationToken.None)
            .Result;

        // Assert
        result.IsError.ShouldBe(false);
        result.Value.ShouldBeOfType<RequestResetPasswordResult>();
        result.Value.Token.ShouldBe(validToken);
    }

    [Fact]
    public void Handle_InvalidRequest_ReturnsError()
    {
        // Arrange
        this._userManagerMock
            .Setup(x => x.FindByEmailAsync(validEmail))!
            .ReturnsAsync(null as User);

        var command = new RequestResetPasswordCommand(validEmail);

        // Act
        var handler = new RequestResetPasswordCommandHandler(
            this._userManagerMock.Object,
            this._emailServiceMock.Object
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
