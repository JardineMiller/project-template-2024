using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.UpdateUser;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IBlobStorageService> _blobStorageServiceMock = new();

    private const string UserId = "0001";

    [Fact]
    public async Task Handle_NoCurrentUser_ReturnsNotFound()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns((string?)null);

        var handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(
            new UpdateUserCommand(),
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
        result
            .Errors.First()
            .Code.ShouldBe(Errors.Common.NotFound(nameof(User)).Code);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);
        _userRepositoryMock
            .Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(
            new UpdateUserCommand(),
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
        result
            .Errors.First()
            .Code.ShouldBe(Errors.Common.NotFound(nameof(User)).Code);
    }

    [Fact]
    public async Task Handle_WithAvatarUrl_ParsesFilename_UpdatesUser_And_ReturnsAvatarUrl()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);

        var user = new User
        {
            Id = UserId,
            AvatarFileName = "old.png",
            Email = "old@example.com",
        };

        _userRepositoryMock
            .Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock
            .Setup(x =>
                x.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);
        _blobStorageServiceMock
            .Setup(x => x.GetAvatarUrl(user.Id, "new.png"))
            .Returns("https://cdn/avatar/new.png");

        var request = new UpdateUserCommand
        {
            AvatarUrl = "https://cdn.example.com/0001/images/new.png",
            DisplayName = "Name",
            Bio = "bio",
        };

        var handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(
            x =>
                x.UpdateUser(
                    It.Is<User>(u => u.AvatarFileName == "new.png"),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        result.Value.AvatarUrl.ShouldBe("https://cdn/avatar/new.png");
    }

    [Fact]
    public async Task Handle_WithEmailChange_UpdatesEmail_NormalizedEmail_UserName_And_ReturnsAvatarUrl()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);

        var user = new User
        {
            Id = UserId,
            AvatarFileName = "old.png",
            Email = "old@example.com",
            NormalizedEmail = "OLD@EXAMPLE.COM",
            UserName = "old@example.com",
        };

        _userRepositoryMock
            .Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock
            .Setup(x =>
                x.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);
        _blobStorageServiceMock
            .Setup(x => x.GetAvatarUrl(user.Id, user.AvatarFileName))
            .Returns("https://cdn/avatar/old.png");

        var request = new UpdateUserCommand
        {
            Email = "new@example.com",
            DisplayName = "Name",
            Bio = "bio",
            AvatarUrl = null,
        };

        var handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(
            x =>
                x.UpdateUser(
                    It.Is<User>(u =>
                        u.Email == "new@example.com"
                        && u.NormalizedEmail == "NEW@EXAMPLE.COM"
                        && u.UserName == "new@example.com"
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        result.Value.User.Email.ShouldBe("new@example.com");
        result.Value.User.NormalizedEmail.ShouldBe("NEW@EXAMPLE.COM");
        result.Value.User.UserName.ShouldBe("new@example.com");
        result.Value.AvatarUrl.ShouldBe("https://cdn/avatar/old.png");
    }
}
