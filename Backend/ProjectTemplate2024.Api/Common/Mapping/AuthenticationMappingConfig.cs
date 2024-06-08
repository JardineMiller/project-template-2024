using Mapster;
using PlanningPoker.Application.Authentication.Commands.Register;
using PlanningPoker.Application.Authentication.Common;
using PlanningPoker.Application.Authentication.Queries.Login;
using PlanningPoker.Contracts.Authentication;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config
            .NewConfig<AuthenticationResult, AuthenticationResponse>()
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.Token),
                dest => dest.Token!
            )
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.PlayerId),
                dest => dest.PlayerId!
            )
            .Map(dest => dest, src => src.User);

        config
            .NewConfig<RegisterCommand, User>()
            .Map(dest => dest.UserName, src => src.Email);
    }
}
