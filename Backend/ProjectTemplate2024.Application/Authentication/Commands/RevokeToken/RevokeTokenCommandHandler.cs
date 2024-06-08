using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.RevokeToken;

public class RevokeTokenCommandHandler
    : IRequestHandler<RevokeTokenCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RevokeTokenCommandHandler(
        UserManager<User> userManager,
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository
    )
    {
        this._userManager = userManager;
        this._dateTimeProvider = dateTimeProvider;
        this._userRepository = userRepository;
    }

    public async Task<ErrorOr<bool>> Handle(
        RevokeTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await this._userRepository.GetUserByRefreshToken(
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

        refreshToken.RevokedOn = this._dateTimeProvider.UtcNow;

        await this._userManager.UpdateAsync(user);

        return true;
    }
}
