using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Commands.UploadUserAvatar;

public class UploadUserAvatarCommandHandlerTests
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
        var fileMock = new Mock<IFormFile>();

        var handler = new UploadUserAvatarCommandHandler(
            _blobStorageServiceMock.Object,
            _currentUserServiceMock.Object,
            _userRepositoryMock.Object
        );

        // Act
        var result = await handler.Handle(
            new UploadUserAvatarCommand(fileMock.Object),
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors.First().Code.ShouldContain("NotFound");
    }

    [Fact]
    public async Task Handle_UploadsFile_PersistsAvatarFileName_And_ReturnsImageUrl()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("new.png");

        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);

        var uploadedUrl = "https://cdn.example.com/0001/images/new.png";

        var user = new User { Id = UserId, AvatarFileName = "old.png" };

        _blobStorageServiceMock
            .Setup(x =>
                x.UploadFile(
                    It.IsAny<IFormFile>(),
                    UserId,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(uploadedUrl);
        _userRepositoryMock
            .Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock
            .Setup(x =>
                x.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);

        var handler = new UploadUserAvatarCommandHandler(
            _blobStorageServiceMock.Object,
            _currentUserServiceMock.Object,
            _userRepositoryMock.Object
        );

        // Act
        var result = await handler.Handle(
            new UploadUserAvatarCommand(fileMock.Object),
            CancellationToken.None
        );

        // Assert
        _blobStorageServiceMock.Verify(
            x =>
                x.UploadFile(
                    It.IsAny<IFormFile>(),
                    UserId,
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        _userRepositoryMock.Verify(
            x =>
                x.UpdateUser(
                    It.Is<User>(u => u.AvatarFileName == "new.png"),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        result.Value.ImageUrl.ShouldBe(uploadedUrl);
    }
}
