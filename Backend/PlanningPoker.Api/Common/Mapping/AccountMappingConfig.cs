using Mapster;
using PlanningPoker.Application.Account.Commands.RequestResetPassword;
using PlanningPoker.Application.Account.Commands.ResetPassword;
using PlanningPoker.Contracts.Account.RequestResetPassword;
using PlanningPoker.Contracts.Account.ResetPassword;

namespace PlanningPoker.Api.Common.Mapping;

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
