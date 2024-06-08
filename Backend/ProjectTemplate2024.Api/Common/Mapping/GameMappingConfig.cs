using Mapster;
using ProjectTemplate2024.Application.Game.Commands.Create;
using ProjectTemplate2024.Application.Game.Queries.GetGame;
using ProjectTemplate2024.Application.Game.Queries.GetUserGames;
using ProjectTemplate2024.Contracts.Game.CreateGame;
using ProjectTemplate2024.Contracts.Game.GetGame;
using ProjectTemplate2024.Contracts.Game.GetUserGames;

namespace ProjectTemplate2024.Api.Common.Mapping;

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
