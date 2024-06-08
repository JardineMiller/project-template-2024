using System.Linq;
using System.Threading;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Game.Queries.GetGame;
using ProjectTemplate2024.Application.Tests.TestHelpers;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Infrastructure.Persistence.Repositories;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Application.Tests.Game.Queries.Get;

public class GetGameQueryHandlerTests
{
    private readonly IGameRepository _gameRepository;
    private readonly string _validGameCode = "1234567A";

    public GetGameQueryHandlerTests()
    {
        var testBase = new QueryTestBase();
        this._gameRepository = new GameRepository(testBase.Context);
    }

    [Fact]
    public void Handle_GivenInvalidId_ReturnsNotFound()
    {
        var query = new GetGameQuery("Code");

        var sut = new GetGameQueryHandler(this._gameRepository);

        var result = sut.Handle(query, CancellationToken.None).Result;

        result.Errors.Count.ShouldBe(1);
        result.Errors
            .First()
            .Code.ShouldBe(Errors.Common.NotFound(nameof(Game)).Code);

        result.Errors
            .First()
            .Description.ShouldBe(
                Errors.Common.NotFound(nameof(Game)).Description
            );
    }

    [Fact]
    public void Handle_GivenValidId_ReturnsGameResult()
    {
        var query = new GetGameQuery(this._validGameCode);

        var sut = new GetGameQueryHandler(this._gameRepository);

        var result = sut.Handle(query, CancellationToken.None).Result;

        result.Value.Code.ShouldBe(this._validGameCode);
    }
}
