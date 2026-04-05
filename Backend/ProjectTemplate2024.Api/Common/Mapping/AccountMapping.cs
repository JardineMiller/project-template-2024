using ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using ProjectTemplate2024.Contracts.Account.RequestResetPassword;
using ProjectTemplate2024.Contracts.Account.ResetPassword;
using ResetPasswordContract = ProjectTemplate2024.Contracts.Account.ResetPassword.ResetPasswordRequest;

namespace ProjectTemplate2024.Api.Common.Mapping;

public static class AccountMapping
{
    public static ResetPasswordCommand ToCommand(this ResetPasswordContract src)
    {
        return new ResetPasswordCommand(
            src.Email.TrimToEmpty(),
            src.NewPassword.TrimToEmpty(),
            src.Token.TrimToNull(),
            src.OldPassword.TrimToNull()
        );
    }

    public static RequestResetPasswordCommand ToCommand(
        this RequestResetPasswordRequest src
    )
    {
        return new RequestResetPasswordCommand(src.Email.TrimToEmpty());
    }

    public static ResetPasswordResponse ToResponse(this ResetPasswordResult src)
    {
        return new ResetPasswordResponse();
    }

    public static RequestResetPasswordResponse ToResponse(
        this RequestResetPasswordResult src
    )
    {
        return new RequestResetPasswordResponse(src.Token);
    }
}
