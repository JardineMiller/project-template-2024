using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Application.Authentication.Common;
using PlanningPoker.Application.Common.Interfaces.Authentication;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoginQueryHandler(
        ITokenGenerator tokenGenerator,
        UserManager<User> userManager,
        IDateTimeProvider dateTimeProvider
    )
    {
        this._tokenGenerator = tokenGenerator;
        this._userManager = userManager;
        this._dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery qry,
        CancellationToken cancellationToken
    )
    {
        var user = this._userManager.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefault(u => u.Email == qry.Email);

        if (user == null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (!user.EmailConfirmed)
        {
            return Errors.Authentication.EmailNotConfirmed;
        }

        if (
            !await this._userManager.CheckPasswordAsync(
                user,
                qry.Password
            )
        )
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var oldRefreshToken = user.RefreshTokens
            .Where(x => x.IsActive)
            .MaxBy(x => x.CreatedOn);

        // replace old refresh token with a new one and save
        var newRefreshToken =
            this._tokenGenerator.GenerateRefreshToken();

        if (oldRefreshToken is not null)
        {
            oldRefreshToken.RevokedOn = this._dateTimeProvider.UtcNow;
            oldRefreshToken.ReplacedBy = newRefreshToken.Token;
        }

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
