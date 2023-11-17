using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Application.Authentication.Common;
using PlanningPoker.Application.Common.Interfaces.Authentication;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler
    : IRequestHandler<
        ConfirmEmailCommand,
        ErrorOr<AuthenticationResult>
    >
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;

    public ConfirmEmailCommandHandler(
        UserManager<User> userManager,
        ITokenGenerator tokenGenerator
    )
    {
        this._userManager = userManager;
        this._tokenGenerator = tokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        ConfirmEmailCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var user = this._userManager.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefault(u => u.Email == cmd.Email);

        if (user == null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var result = await this._userManager.ConfirmEmailAsync(
            user,
            cmd.Token
        );

        if (!result.Succeeded)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = this._tokenGenerator.GenerateJwt(user);

        // replace old refresh token with a new one and save
        var newRefreshToken =
            this._tokenGenerator.GenerateRefreshToken();

        user.RefreshTokens.Add(newRefreshToken);
        await this._userManager.UpdateAsync(user);

        return new AuthenticationResult(
            user,
            token,
            newRefreshToken.Token
        );
    }
}
