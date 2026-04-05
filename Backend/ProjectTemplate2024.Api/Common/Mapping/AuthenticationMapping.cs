using System;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using ProjectTemplate2024.Contracts.Authentication;

namespace ProjectTemplate2024.Api.Common.Mapping;

public static class AuthenticationMapping
{
    public static RegisterCommand ToCommand(this RegisterRequest src)
    {
        return new RegisterCommand(
            src.DisplayName.TrimToEmpty(),
            src.Email.TrimToEmpty(),
            src.Password.TrimToEmpty()
        );
    }

    public static LoginQuery ToQuery(this LoginRequest src)
    {
        return new LoginQuery(
            src.Email.TrimToEmpty(),
            src.Password.TrimToEmpty()
        );
    }

    public static AuthenticationResponse ToResponse(
        this AuthenticationResult src
    )
    {
        var id = Guid.TryParse(src.User.Id, out var parsed)
            ? parsed
            : Guid.Empty;
        var token = string.IsNullOrWhiteSpace(src.Token) ? null : src.Token;
        var avatar = string.IsNullOrWhiteSpace(src.AvatarUrl)
            ? null
            : src.AvatarUrl;

        return new AuthenticationResponse(
            id,
            src.User.DisplayName,
            src.User.Email,
            token,
            avatar
        );
    }
}
