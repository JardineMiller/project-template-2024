using System;
using System.Collections.Generic;
using Mapster;
using PlanningPoker.Api.Common.Mapping;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Application.Game.Queries.GetGame;
using PlanningPoker.Application.Game.Queries.GetUserGames;
using PlanningPoker.Application.Tests.TestHelpers;
using PlanningPoker.Contracts.Game.CreateGame;
using PlanningPoker.Contracts.Game.GetGame;
using PlanningPoker.Contracts.Game.GetUserGames;
using PlanningPoker.Domain.Common.Validation;
using PlanningPoker.Infrastructure.Generators;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Api.Tests.Common.Mapping;

public class GameMappingConfigTests
{
    private readonly string _validName =
        TestDataGenerator.GenerateRandomString(
            Validation.Game.Name.MaxLength
        );

    private readonly string _validDescription =
        TestDataGenerator.GenerateRandomString(
            Validation.Game.Description.MaxLength
        );

    private readonly string _validOwnerId = Guid.NewGuid().ToString();

    public GameMappingConfigTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        GameMappingConfig.AddConfig(config);
    }

    [Fact]
    public void CreateGameRequest_ShouldMapTo_CreateGameCommand()
    {
        var src = new CreateGameRequest(
            this._validName,
            this._validDescription,
            this._validOwnerId
        );

        var result = src.Adapt<CreateGameCommand>();

        result.Name.ShouldBe(this._validName);
        result.Description.ShouldBe(this._validDescription);
        result.OwnerId.ShouldBe(this._validOwnerId);
    }

    [Fact]
    public void CreateGameResult_ShouldMapTo_CreateGameResponse()
    {
        var generator = new TinyGuidGenerator();
        var code = generator.Generate();

        var src = new CreateGameResult(code);

        var result = src.Adapt<CreateGameResponse>();

        result.GameCode.ShouldBe(code);
    }

    [Fact]
    public void GetGameRequest_ShouldMapTo_GetGameCommand()
    {
        var generator = new TinyGuidGenerator();
        var code = generator.Generate();

        var src = new GetGameRequest(code);
        var result = src.Adapt<GetGameRequest>();

        result.Code.ShouldBe(code);
    }

    [Fact]
    public void GetGameResult_ShouldMapTo_GetGameResponse()
    {
        var generator = new TinyGuidGenerator();
        var code = generator.Generate();

        var src = new GetGameResult(
            "Name",
            "Description",
            code,
            "OwnerId",
            null
        );

        var result = src.Adapt<GetGameResponse>();

        result.Name.ShouldBe(src.Name);
        result.Description.ShouldBe(src.Description);
        result.Code.ShouldBe(src.Code);
        result.OwnerId.ShouldBe(src.OwnerId);
        result.Owner.ShouldBe(src.Owner);
    }

    [Fact]
    public void GetUserGamesResult_ShouldMapTo_GetUserGamesResponse()
    {
        // Arrange
        var ownerId = "Owner-1";

        var games = new List<Domain.Entities.Game>
        {
            new()
            {
                Code = "GameCode-1",
                OwnerId = ownerId,
                Name = "Name-1"
            },
            new()
            {
                Code = "GameCode-2",
                OwnerId = ownerId,
                Name = "Name-2"
            }
        };

        var src = new GetUserGamesResult(ownerId, games);

        // Act
        var result = src.Adapt<GetUserGamesResponse>();

        // Assert
        result.Games.ShouldBeEquivalentTo(src.Games);
        result.UserId.ShouldBe(src.UserId);
    }
}
