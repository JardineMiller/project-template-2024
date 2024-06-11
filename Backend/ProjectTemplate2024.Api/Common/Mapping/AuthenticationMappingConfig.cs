using Mapster;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using ProjectTemplate2024.Contracts.Authentication;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Api.Common.Mapping;

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
            .Map(dest => dest, src => src.User)
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.AvatarUrl),
                dest => dest.AvatarUrl!
            );

        config
            .NewConfig<RegisterCommand, User>()
            .Map(dest => dest.UserName, src => src.Email);
    }
}
