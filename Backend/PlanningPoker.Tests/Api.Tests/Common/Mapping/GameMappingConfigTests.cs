using System;
using Mapster;
using PlanningPoker.Api.Common.Mapping;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Application.Tests.TestHelpers;
using PlanningPoker.Contracts.Game;
using PlanningPoker.Domain.Common.Validation;
using PlanningPoker.Infrastructure.Generators;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Api.Tests.Common.Mapping;

public class GameMappingConfigTests
{
    private readonly string _validName =
        TestDataGenerator.GenerateRandomString(
            ValidationConstants.Game.Name.MaxLength
        );

    private readonly string _validDescription =
        TestDataGenerator.GenerateRandomString(
            ValidationConstants.Game.Description.MaxLength
        );

    private readonly string validOwnerId = Guid.NewGuid().ToString();

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
            this.validOwnerId
        );

        var result = src.Adapt<CreateGameCommand>();

        result.Name.ShouldBe(this._validName);
        result.Description.ShouldBe(this._validDescription);
        result.OwnerId.ShouldBe(this.validOwnerId);
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
}
