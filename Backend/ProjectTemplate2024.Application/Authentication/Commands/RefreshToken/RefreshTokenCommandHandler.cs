using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshTokenCommandHandler(
        UserManager<User> userManager,
        ITokenGenerator tokenGenerator,
        IDateTimeProvider dateTimeProvider
    )
    {
        this._userManager = userManager;
        this._tokenGenerator = tokenGenerator;
        this._dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = this._userManager.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefault(
                u => u.RefreshTokens.Any(t => t.Token == request.Token)
            );

        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var oldRefreshToken = user.RefreshTokens.Single(
            x => x.Token == request.Token
        );

        if (!oldRefreshToken.IsActive)
        {
            return Errors.Authentication.TokenExpired;
        }

        // replace old refresh token with a new one and save
        var newRefreshToken = this._tokenGenerator.GenerateRefreshToken();

        oldRefreshToken.RevokedOn = this._dateTimeProvider.UtcNow;
        oldRefreshToken.ReplacedBy = newRefreshToken.Token;

        user.RefreshTokens.Add(newRefreshToken);
        await this._userManager.UpdateAsync(user);

        // generate new jwt
        var jwt = this._tokenGenerator.GenerateJwt(user);

        var response = new AuthenticationResult(
            user,
            jwt,
            newRefreshToken.Token
        );

        return response;
    }
}
