using Mapster;
using ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using ProjectTemplate2024.Contracts.Account.RequestResetPassword;
using ProjectTemplate2024.Contracts.Account.ResetPassword;

namespace ProjectTemplate2024.Api.Common.Mapping;

public class AccountMappingConfig
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<ResetPasswordRequest, ResetPasswordCommand>()
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.Token),
                dest => dest.Token!
            )
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.OldPassword),
                dest => dest.OldPassword!
            );

        config.NewConfig<
            ResetPasswordResult,
            ResetPasswordResponse
        >();

        config.NewConfig<
            RequestResetPasswordRequest,
            RequestResetPasswordCommand
        >();

        config.NewConfig<
            RequestResetPasswordResult,
            RequestResetPasswordResponse
        >();
    }
}
