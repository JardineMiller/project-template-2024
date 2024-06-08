using Mapster;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Application.Game.Queries.GetGame;
using PlanningPoker.Application.Game.Queries.GetUserGames;
using PlanningPoker.Contracts.Game.CreateGame;
using PlanningPoker.Contracts.Game.GetGame;
using PlanningPoker.Contracts.Game.GetUserGames;

namespace PlanningPoker.Api.Common.Mapping;

public class GameMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateGameRequest, CreateGameCommand>();
        config.NewConfig<CreateGameResult, CreateGameResponse>();

        config.NewConfig<GetGameResult, GetGameResponse>();
        config.NewConfig<GetUserGamesResult, GetUserGamesResponse>();
    }
}
