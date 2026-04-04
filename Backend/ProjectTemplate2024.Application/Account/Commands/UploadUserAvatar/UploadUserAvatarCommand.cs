using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;

public class UploadUserAvatarCommand : IRequest<ErrorOr<UploadUserAvatarResult>>
{
    public IFormFile File { get; set; }

    public UploadUserAvatarCommand(IFormFile file)
    {
        File = file;
    }
}
