using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;

namespace ProjectTemplate2024.Application.Account.Commands.DeleteAvatar;

public class DeleteAvatarCommand : IRequest<ErrorOr<UpdateUserResult>>
{
    public string FileName { get; set; } = string.Empty;

    public DeleteAvatarCommand(string fileName)
    {
        FileName = fileName ?? string.Empty;
    }
}
