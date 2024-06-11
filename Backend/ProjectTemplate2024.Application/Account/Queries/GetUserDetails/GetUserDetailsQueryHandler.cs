using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Queries.GetUserDetails;

public class GetUserDetailsQueryHandler
    : IRequestHandler<GetUserDetailsQuery, ErrorOr<GetUserDetailsResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobStorageService _blobStorageService;

    public GetUserDetailsQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IBlobStorageService blobStorageService
    )
    {
        this._userRepository = userRepository;
        this._currentUserService = currentUserService;
        this._blobStorageService = blobStorageService;
    }

    public async Task<ErrorOr<GetUserDetailsResult>> Handle(
        GetUserDetailsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = this._currentUserService.UserId;

        if (userId is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var user = await this._userRepository.GetUserById(
            userId,
            cancellationToken
        );

        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        return new GetUserDetailsResult(
            user,
            this._blobStorageService.GetAvatarUrl(user.Id, user.AvatarFileName)
        );
    }
}
