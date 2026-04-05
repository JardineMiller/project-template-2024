using Mapster;
using ProjectTemplate2024.Application.Account.Commands.DeleteAvatar;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.UpdateUser;

namespace ProjectTemplate2024.Api.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<GetUserDetailsResult, GetUserDetailsResponse>()
            .Map(dest => dest.Id, src => src.User.Id)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.DisplayName, src => src.User.DisplayName)
            .Map(dest => dest.Bio, src => src.User.Bio)
            .Map(dest => dest.AvatarUrl, src => src.AvatarUrl)
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.User.Bio),
                dest => dest.Bio!
            )
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.AvatarUrl),
                dest => dest.AvatarUrl!
            );

        config
            .NewConfig<UpdateUserResult, UpdateUserResponse>()
            .Map(dest => dest.Id, src => src.User.Id)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.DisplayName, src => src.User.DisplayName)
            .Map(dest => dest.Bio, src => src.User.Bio)
            .Map(dest => dest.AvatarUrl, src => src.AvatarUrl)
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.User.Bio),
                dest => dest.Bio!
            )
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.AvatarUrl),
                dest => dest.AvatarUrl!
            );
        config.NewConfig<UploadUserAvatarResult, UploadUserAvatarResult>();
    }
}
