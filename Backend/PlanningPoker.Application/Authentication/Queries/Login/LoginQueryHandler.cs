using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PlanningPoker.Application.Authentication.Common;
using PlanningPoker.Application.Common.Interfaces.Authentication;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginQueryHandler(
        IJwtGenerator jwtGenerator,
        UserManager<User> userManager
    )
    {
        this._jwtGenerator = jwtGenerator;
        this._userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery qry,
        CancellationToken cancellationToken
    )
    {
        var user = await this._userManager.FindByEmailAsync(
            qry.Email
        );

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

        var token = this._jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
