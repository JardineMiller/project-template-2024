using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Application.Game.Queries.GetUserGames;
using PlanningPoker.Application.Tests.TestHelpers;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;
using PlanningPoker.Infrastructure.Persistence.Repositories;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Application.Tests.Game.Queries.GetUserGames;

public class GetUsersGamesHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly IGameRepository _gameRepository;

    public GetUsersGamesHandlerTests()
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

        this._userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))!
            .ReturnsAsync(null as User);

        var testBase = new QueryTestBase();
        this._gameRepository = new GameRepository(testBase.Context);
    }

    [Fact]
    public async Task Handle_GivenInvalidUserId_Throws_Error()
    {
        // Arrange
        var invalidUserId = "invalid-Id";

        this._userManagerMock
            .Setup(x => x.FindByIdAsync(invalidUserId))!
            .ReturnsAsync(null as User);

        var handler = new GetUserGamesQueryHandler(
            this._gameRepository,
            this._userManagerMock.Object
        );

        // Act
        var result = await handler.Handle(
            new GetUserGamesQuery(invalidUserId),
            CancellationToken.None
        );

        // Assert
        result.Errors.Count.ShouldNotBe(0);

        var userNotFoundError = Errors.Common.NotFound(nameof(User));

        result.Errors.First().Code.ShouldBe(userNotFoundError.Code);

        result.Errors
            .First()
            .Description.ShouldBe(userNotFoundError.Description);
    }

    [Fact]
    public async Task Handle_GivenValidInput_ReturnsResponse()
    {
        // Arrange
        var validUserId = "0001";

        this._userManagerMock
            .Setup(x => x.FindByIdAsync(validUserId))!
            .ReturnsAsync(new User { Id = "0001" });

        var handler = new GetUserGamesQueryHandler(
            this._gameRepository,
            this._userManagerMock.Object
        );

        // Act
        var result = await handler.Handle(
            new GetUserGamesQuery(validUserId),
            CancellationToken.None
        );

        // Assert
        result.Value.ShouldBeOfType<GetUserGamesResult>();
    }
}
