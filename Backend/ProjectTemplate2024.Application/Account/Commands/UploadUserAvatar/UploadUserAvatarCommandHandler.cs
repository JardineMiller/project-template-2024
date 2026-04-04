using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;

public class UploadUserAvatarCommandHandler
    : IRequestHandler<UploadUserAvatarCommand, ErrorOr<UploadUserAvatarResult>>
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public UploadUserAvatarCommandHandler(
        IBlobStorageService blobStorageService,
        ICurrentUserService currentUserService,
        IUserRepository userRepository
    )
    {
        _blobStorageService = blobStorageService;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UploadUserAvatarResult>> Handle(
        UploadUserAvatarCommand request,
        CancellationToken cancellationToken
    )
    {
        // Validate file size
        // Validate file type
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Errors.Common.NotFound(
                $"{nameof(User)} not found [{userId}]"
            );
        }

        var result = await _blobStorageService.UploadFile(
            request.File,
            userId,
            cancellationToken
        );

        if (result is null)
        {
            return Errors.Common.NotFound(
                "Something went wrong uploading the file."
            );
        }

        // Persist avatar file name on the user so the avatar is immediately available
        var user = await _userRepository.GetUserById(userId, cancellationToken);

        if (user is not null)
        {
            // Use the uploaded file name (browser provides filename) to persist
            user.AvatarFileName = request.File.FileName;

            await _userRepository.UpdateUser(user, cancellationToken);
        }

        return new UploadUserAvatarResult(result);
    }
}
