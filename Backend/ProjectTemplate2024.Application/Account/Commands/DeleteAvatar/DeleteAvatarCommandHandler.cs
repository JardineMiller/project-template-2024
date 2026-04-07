using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Commands.DeleteAvatar;

public class DeleteAvatarCommandHandler
    : IRequestHandler<DeleteAvatarCommand, ErrorOr<UpdateUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobStorageService _blobStorageService;

    public DeleteAvatarCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IBlobStorageService blobStorageService
    )
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _blobStorageService = blobStorageService;
    }

    public async Task<ErrorOr<UpdateUserResult>> Handle(
        DeleteAvatarCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var user = await _userRepository.GetUserById(userId, cancellationToken);

        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var fileName = request.FileName;

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            // If a full URL was passed, attempt to extract just the file name
            try
            {
                if (fileName.Contains('/'))
                {
                    var uri = new Uri(fileName);
                    fileName = Path.GetFileName(uri.LocalPath);
                }
            }
            catch
            {
                // ignore and use provided string as-is
            }

            await _blobStorageService.DeleteFile(
                userId,
                fileName,
                cancellationToken
            );
        }

        user.AvatarFileName = null;

        await _userRepository.UpdateUser(user, cancellationToken);

        return new UpdateUserResult(user, user.AvatarFileName);
    }
}
