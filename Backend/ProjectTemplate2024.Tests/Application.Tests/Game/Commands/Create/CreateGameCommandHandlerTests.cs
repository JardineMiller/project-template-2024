using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using ProjectTemplate2024.Application.Common.Interfaces.Generators;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Game.Commands.Create;
using ProjectTemplate2024.Application.Tests.TestHelpers;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using ProjectTemplate2024.Infrastructure.Persistence.Repositories;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Game.Commands.Create;

public class CreateGameCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IGameRepository _gameRepository;
    private readonly Mock<ITinyGuidGenerator> _tinyGuidGeneratorMock =
        new();

    private const string validName = "Test Name";
    private const string validDescription = "Test Description";
    private const string newGameCode = "a1b2c3d4";
    private readonly string _validOwnerId;

    public CreateGameCommandHandlerTests()
    {
        var testBase = new CommandTestBase();
        this._gameRepository = new GameRepository(testBase.Context);

        this._tinyGuidGeneratorMock
            .Setup(x => x.Generate())
            .Returns(newGameCode);

        this._userManager = new UserManager<User>(
            new UserStore<User>(testBase.Context),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        this._validOwnerId = testBase.Context.Users
            .FirstOrDefault()!
            .Id;
    }

    [Fact]
    public void Handle_GivenNonExistingEmail_ReturnsInvalidCredError()
    {
        // Arrange
        var command = new CreateGameCommand(
            validName,
            validDescription,
            "invalidId"
        );

        var sut = new CreateGameCommandHandler(
            this._userManager,
            this._gameRepository,
            this._tinyGuidGeneratorMock.Object
        );

        // Act
        var result = sut.Handle(
            command,
            CancellationToken.None
        ).Result;

        // Assert
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(Errors.Common.NotFound(nameof(User)).Code);

        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Common.NotFound(nameof(User)).Description
            );
    }

    [Fact]
    public void Handle_GivenValidInput_ReturnsNewGameCode()
    {
        // Arrange
        var command = new CreateGameCommand(
            validName,
            validDescription,
            this._validOwnerId
        );

        var sut = new CreateGameCommandHandler(
            this._userManager,
            this._gameRepository,
            this._tinyGuidGeneratorMock.Object
        );

        // Act
        var result = sut.Handle(
            command,
            CancellationToken.None
        ).Result;

        // Assert
        result.Value.GameCode.ShouldBe(newGameCode);
    }
}
