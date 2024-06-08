using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.RevokeToken;

public class RevokeTokenCommandHandler
    : IRequestHandler<RevokeTokenCommand, ErrorOr<bool>>
{
    private readonly UserManager<User> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RevokeTokenCommandHandler(
        UserManager<User> userManager,
        IDateTimeProvider dateTimeProvider
    )
    {
        this._userManager = userManager;
        this._dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<bool>> Handle(
        RevokeTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = this._userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(
                u =>
                    u.RefreshTokens.Any(t => t.Token == request.Token)
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
