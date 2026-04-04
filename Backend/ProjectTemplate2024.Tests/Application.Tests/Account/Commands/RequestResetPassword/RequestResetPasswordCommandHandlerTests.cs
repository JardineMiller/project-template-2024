using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.RequestResetPassword;

public class RequestResetPasswordCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock;

    private const string ValidEmail = "test2@email.com";
    private const string ValidToken = "validToken";

    public RequestResetPasswordCommandHandlerTests()
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

        _emailServiceMock = new Mock<IEmailService>();
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(new User { Email = ValidEmail });

        _userManagerMock
            .Setup(
                x =>
                    x.GeneratePasswordResetTokenAsync(
                        It.IsAny<User>()
                    )
            )
            .ReturnsAsync(ValidToken);

        var command = new RequestResetPasswordCommand(ValidEmail);

        // Act
        var handler = new RequestResetPasswordCommandHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBe(false);
        result.Value.ShouldBeOfType<RequestResetPasswordResult>();
        result.Value.Token.ShouldBe(ValidToken);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(null as User);

        var command = new RequestResetPasswordCommand(ValidEmail);

        // Act
        var handler = new RequestResetPasswordCommandHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object
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
