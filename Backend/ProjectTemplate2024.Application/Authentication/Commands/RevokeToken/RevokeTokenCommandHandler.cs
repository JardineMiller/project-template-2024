using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.RevokeToken;

public class RevokeTokenCommandHandler
    : IRequestHandler<RevokeTokenCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RevokeTokenCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository
    )
    {
        _dateTimeProvider = dateTimeProvider;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<bool>> Handle(
        RevokeTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetUserByRefreshToken(
            request.Token,
            cancellationToken
        );

        // return error if no user found with token
        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var refreshToken = user.RefreshTokens.Single(
            x => x.Token == request.Token
        );

        if (!refreshToken.IsActive)
        {
            return Errors.Authentication.TokenExpired;
        }

        refreshToken.RevokedOn = _dateTimeProvider.UtcNow;

        await _userRepository.UpdateUser(user, cancellationToken);

        return true;
    }
}
