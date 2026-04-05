using System;
using ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.RequestResetPassword;
using ProjectTemplate2024.Contracts.Account.ResetPassword;
using ProjectTemplate2024.Contracts.Account.UpdateUser;
using ProjectTemplate2024.Contracts.Account.UploadUserAvatar;
using ProjectTemplate2024.Contracts.Authentication;

namespace ProjectTemplate2024.Api.Common.Mapping;

public static class MappingExtensions
{
    // Contracts -> Application (requests -> commands/queries)
    public static RegisterCommand ToCommand(this RegisterRequest src)
    {
        return new RegisterCommand(
            src.DisplayName?.Trim() ?? string.Empty,
            src.Email?.Trim() ?? string.Empty,
            src.Password?.Trim() ?? string.Empty
        );
    }

    public static LoginQuery ToQuery(this LoginRequest src)
    {
        return new LoginQuery(
            src.Email?.Trim() ?? string.Empty,
            src.Password?.Trim() ?? string.Empty
        );
    }

    public static UpdateUserCommand ToCommand(this UpdateUserRequest src)
    {
        return new UpdateUserCommand
        {
            Email = src.Email?.Trim() ?? string.Empty,
            DisplayName = src.DisplayName?.Trim() ?? string.Empty,
            Bio = string.IsNullOrWhiteSpace(src.Bio) ? null : src.Bio?.Trim(),
            AvatarUrl = string.IsNullOrWhiteSpace(src.AvatarUrl)
                ? null
                : src.AvatarUrl?.Trim(),
        };
    }

    public static ResetPasswordCommand ToCommand(
        this ProjectTemplate2024.Contracts.Account.ResetPassword.ResetPasswordRequest src
    )
    {
        return new ResetPasswordCommand(
            src.Email?.Trim() ?? string.Empty,
            src.NewPassword?.Trim() ?? string.Empty,
            string.IsNullOrWhiteSpace(src.Token) ? null : src.Token?.Trim(),
            string.IsNullOrWhiteSpace(src.OldPassword)
                ? null
                : src.OldPassword?.Trim()
        );
    }

    public static RequestResetPasswordCommand ToCommand(
        this RequestResetPasswordRequest src
    )
    {
        return new RequestResetPasswordCommand(
            src.Email?.Trim() ?? string.Empty
        );
    }

    // Application -> Contracts (results -> responses)
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

    public static GetUserDetailsResponse ToResponse(
        this GetUserDetailsResult src
    )
    {
        var bio = string.IsNullOrWhiteSpace(src.User.Bio) ? null : src.User.Bio;
        var avatar = string.IsNullOrWhiteSpace(src.AvatarUrl)
            ? null
            : src.AvatarUrl;

        return new GetUserDetailsResponse(
            src.User.Id,
            src.User.DisplayName,
            src.User.Email,
            bio,
            avatar
        );
    }

    public static UpdateUserResponse ToResponse(
        this ProjectTemplate2024.Application.Account.Commands.UpdateUser.UpdateUserResult src
    )
    {
        var bio = string.IsNullOrWhiteSpace(src.User.Bio) ? null : src.User.Bio;
        var avatar = string.IsNullOrWhiteSpace(src.AvatarUrl)
            ? null
            : src.AvatarUrl;

        return new UpdateUserResponse(
            src.User.Id,
            src.User.DisplayName,
            src.User.Email,
            bio,
            avatar
        );
    }

    public static UploadUserAvatarResponse ToResponse(
        this UploadUserAvatarResult src
    )
    {
        return new UploadUserAvatarResponse(src.ImageUrl);
    }

    public static ResetPasswordResponse ToResponse(this ResetPasswordResult src)
    {
        return new ResetPasswordResponse();
    }

    public static RequestResetPasswordResponse ToResponse(
        this ProjectTemplate2024.Application.Account.Commands.RequestResetPassword.RequestResetPasswordResult src
    )
    {
        return new RequestResetPasswordResponse(src.Token);
    }
}
