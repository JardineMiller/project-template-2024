using Mapster;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Contracts.Game;

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
    }
}
