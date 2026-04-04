using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Authentication.Commands.Register;

public class RegisterCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock;

    private const string ValidDisplayName = "Test User";
    private const string ValidEmail = "test2@email.com";
    private const string ValidPassword = "Password123!";

    public RegisterCommandHandlerTests()
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
    public async Task Handle_GivenValidRequest_ReturnsCorrectResponse()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(null as User);

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var command = new RegisterCommand(
            ValidDisplayName,
            ValidEmail,
            ValidPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Token.ShouldBe(null);
        result.Value.User.DisplayName.ShouldBe(ValidDisplayName);
        result.Value.User.Email.ShouldBe(ValidEmail);
    }

    [Fact]
    public async Task Handle_GivenExistingUserEmail_ShouldReturnUserAuthError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(new User { Email = ValidEmail });

        var command = new RegisterCommand(
            ValidDisplayName,
            ValidEmail,
            ValidPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors.First().Code.ShouldBe(Errors.User.DuplicateEmail.Code);
        result.Errors
            .First()
            .Description.ShouldBe(Errors.User.DuplicateEmail.Description);
    }

    [Fact]
    public async Task Handle_GivenUserCreationFailed_ShouldReturnCreationFailedError()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(ValidEmail))
            .ReturnsAsync(null as User);

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        var command = new RegisterCommand(
            ValidDisplayName,
            ValidEmail,
            ValidPassword
        );

        // Act
        var handler = new RegisterCommandHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors.First().Code.ShouldBe(Errors.User.CreationFailed.Code);
        result.Errors
            .First()
            .Description.ShouldBe(Errors.User.CreationFailed.Description);
    }
}
