using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using System.IO;
using System;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Commands.UpdateUser;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, ErrorOr<UpdateUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobStorageService _blobStorageService;

    public UpdateUserCommandHandler(
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
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var user = await _userRepository.GetUserById(
            userId,
            cancellationToken
        );

        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        user.DisplayName = request.DisplayName;
        user.Bio = request.Bio;

        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToUpperInvariant();
            user.UserName = request.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.AvatarUrl))
        {
            try
            {
                var uri = new Uri(request.AvatarUrl);
                var fileName = Path.GetFileName(uri.LocalPath);

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    user.AvatarFileName = fileName;
                }
            }
            catch
            {
                // ignore invalid avatar url
            }
        }

        await _userRepository.UpdateUser(user, cancellationToken);

        return new UpdateUserResult(
            user,
            _blobStorageService.GetAvatarUrl(user.Id, user.AvatarFileName)
        );
    }
}
