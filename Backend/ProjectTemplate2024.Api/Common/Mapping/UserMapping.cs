using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.UpdateUser;
using ProjectTemplate2024.Contracts.Account.UploadUserAvatar;

namespace ProjectTemplate2024.Api.Common.Mapping;

public static class UserMapping
{
    public static UpdateUserCommand ToCommand(this UpdateUserRequest src)
    {
        return new UpdateUserCommand
        {
            Email = src.Email.TrimToEmpty(),
            DisplayName = src.DisplayName.TrimToEmpty(),
            Bio = src.Bio.TrimToNull(),
            AvatarUrl = src.AvatarUrl.TrimToNull(),
        };
    }

    public static GetUserDetailsResponse ToResponse(
        this GetUserDetailsResult src
    )
    {
        var bio = src.User.Bio.TrimToNull();
        var avatar = src.AvatarUrl.TrimToNull();

        return new GetUserDetailsResponse(
            src.User.Id,
            src.User.DisplayName,
            src.User.Email,
            bio,
            avatar
        );
    }

    public static UpdateUserResponse ToResponse(this UpdateUserResult src)
    {
        var bio = src.User.Bio.TrimToNull();
        var avatar = src.AvatarUrl.TrimToNull();

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
}
