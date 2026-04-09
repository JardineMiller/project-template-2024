using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ProjectTemplate2024.Application.Account.Commands.DeleteAvatar;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.DeleteAvatar;

public class DeleteAvatarCommandHandlerTests
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

        var handler = new DeleteAvatarCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(
            new DeleteAvatarCommand("file.png"),
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

        var handler = new DeleteAvatarCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(
            new DeleteAvatarCommand("file.png"),
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
        result
            .Errors.First()
            .Code.ShouldBe(Errors.Common.NotFound(nameof(User)).Code);
    }

    [Fact]
    public async Task Handle_WithFileUrl_DeletesBlob_And_UpdatesUser()
    {
        // Arrange
        var user = new User { Id = UserId, AvatarFileName = "old.png" };

        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);
        _userRepositoryMock
            .Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _blobStorageServiceMock
            .Setup(x =>
                x.DeleteFile(UserId, "new.png", It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        var handler = new DeleteAvatarCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        var fullUrl = "https://cdn.example.com/0001/images/new.png";

        // Act
        var result = await handler.Handle(
            new DeleteAvatarCommand(fullUrl),
            CancellationToken.None
        );

        // Assert
        _blobStorageServiceMock.Verify(
            x => x.DeleteFile(UserId, "new.png", It.IsAny<CancellationToken>()),
            Times.Once
        );
        _userRepositoryMock.Verify(
            x =>
                x.UpdateUser(
                    It.Is<User>(u => u.AvatarFileName == null),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        result.Value.AvatarUrl.ShouldBeNull();
    }
}
