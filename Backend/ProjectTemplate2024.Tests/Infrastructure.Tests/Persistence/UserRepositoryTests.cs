using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Tests.TestHelpers;
using ProjectTemplate2024.Domain.Entities;
using ProjectTemplate2024.Infrastructure.Persistence.Repositories;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Infrastructure.Tests.Persistence;

public class UserRepositoryTests : QueryTestBase
{
    [Fact]
    public async Task GetUserByEmail_Returns_User_When_Found()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<User>>(
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

        userManagerMock.Setup(x => x.Users).Returns(Context.Users);

        var repo = new UserRepository(userManagerMock.Object);

        // Act
        var user = await repo.GetUserByEmail(
            "user1@test.com",
            CancellationToken.None
        );

        // Assert
        user.ShouldNotBeNull();
        user!.Id.ShouldBe("0001");
    }

    [Fact]
    public async Task UpdateUser_Invokes_UserManager_UpdateAsync()
    {
        var userManagerMock = new Mock<UserManager<User>>(
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

        userManagerMock.Setup(x => x.Users).Returns(Context.Users);

        var repo = new UserRepository(userManagerMock.Object);

        var user = Context.Users.First();

        userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        // Act
        await repo.UpdateUser(user, CancellationToken.None);

        // Assert
        userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task CheckPasswordAsync_Delegates_To_UserManager()
    {
        var userManagerMock = new Mock<UserManager<User>>(
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

        userManagerMock.Setup(x => x.Users).Returns(Context.Users);

        var repo = new UserRepository(userManagerMock.Object);

        var user = Context.Users.First();

        userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, "secret"))
            .ReturnsAsync(true);

        var result = await repo.CheckPasswordAsync(
            user,
            "secret",
            CancellationToken.None
        );

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task GetUserByRefreshToken_Returns_User_When_Matching_Token()
    {
        var userManagerMock = new Mock<UserManager<User>>(
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

        // add a refresh token to one of the seeded users
        var user = Context.Users.First();
        var token = new RefreshToken { Token = "refresh-token-1" };
        user.RefreshTokens.Add(token);
        Context.SaveChanges();

        userManagerMock.Setup(x => x.Users).Returns(Context.Users);

        var repo = new UserRepository(userManagerMock.Object);

        var found = await repo.GetUserByRefreshToken(
            "refresh-token-1",
            CancellationToken.None
        );

        found.ShouldNotBeNull();
        found!.Id.ShouldBe(user.Id);
    }
}
