using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Account.Queries.GetUserDetails;

public class GetUserDetailsQueryHandlerTests
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

        var handler = new GetUserDetailsQueryHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(new GetUserDetailsQuery(), CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors.First().Code.ShouldBe(Errors.Common.NotFound(nameof(User)).Code);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);
        _userRepositoryMock.Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var handler = new GetUserDetailsQueryHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(new GetUserDetailsQuery(), CancellationToken.None);

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors.First().Code.ShouldBe(Errors.Common.NotFound(nameof(User)).Code);
    }

    [Fact]
    public async Task Handle_WithExistingUser_ReturnsUserDetails_WithAvatarUrl()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns(UserId);

        var user = new User { Id = UserId, AvatarFileName = null };
        _userRepositoryMock.Setup(x => x.GetUserById(UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _blobStorageServiceMock.Setup(x => x.GetAvatarUrl(user.Id, user.AvatarFileName)).Returns("https://cdn/avatar/null");

        var handler = new GetUserDetailsQueryHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _blobStorageServiceMock.Object
        );

        // Act
        var result = await handler.Handle(new GetUserDetailsQuery(), CancellationToken.None);

        // Assert
        result.Value.AvatarUrl.ShouldBe("https://cdn/avatar/null");
    }

}
