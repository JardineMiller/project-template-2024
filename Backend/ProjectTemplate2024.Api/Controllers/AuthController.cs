using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate2024.Application.Authentication.Commands.ConfirmEmail;
using ProjectTemplate2024.Application.Authentication.Commands.RefreshToken;
using ProjectTemplate2024.Application.Authentication.Commands.Register;
using ProjectTemplate2024.Application.Authentication.Commands.RevokeToken;
using ProjectTemplate2024.Application.Authentication.Queries.Login;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Contracts.Authentication;

namespace ProjectTemplate2024.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuthController(
        ISender mediator,
        IDateTimeProvider dateTimeProvider
    )
    {
        this._mediator = mediator;
        this._dateTimeProvider = dateTimeProvider;
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var cmd = request.Adapt<RegisterCommand>();
        var authResult = await this._mediator.Send(cmd);

        return authResult.Match(
            result => Ok(result.Adapt<AuthenticationResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var qry = request.Adapt<LoginQuery>();
        var authResult = await this._mediator.Send(qry);

        return authResult.Match(
            result =>
            {
                SetTokenCookie(result.RefreshToken!);
                return Ok(result.Adapt<AuthenticationResponse>());
            },
            errors => Problem(errors)
        );
    }

    [HttpGet(nameof(RefreshToken))]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = this.Request.Cookies["refreshToken"];

        if (refreshToken is null)
        {
            return NoContent();
        }

        var cmd = new RefreshTokenCommand(refreshToken);

        var result = await this._mediator.Send(cmd);

        return result.Match(
            authResult =>
            {
                SetTokenCookie(authResult.RefreshToken!);
                return Ok(authResult.Adapt<AuthenticationResponse>());
            },
            errors =>
            {
                RemoveCookie();
                return Problem(errors);
            }
        );
    }

    [HttpGet(nameof(RevokeToken))]
    public async Task<IActionResult> RevokeToken()
    {
        var refreshToken = this.Request.Cookies["refreshToken"];
        var cmd = new RevokeTokenCommand(refreshToken!);

        RemoveCookie();

        var result = await this._mediator.Send(cmd);

        return result.Match(_ => Ok(), errors => Problem(errors));
    }

    [HttpGet(nameof(Confirm))]
    public async Task<IActionResult> Confirm(
        string token,
        string email
    )
    {
        var cmd = new ConfirmEmailCommand(email, token);
        var authResult = await this._mediator.Send(cmd);

        return authResult.Match(
            result =>
            {
                SetTokenCookie(result.RefreshToken!);
                return Ok(result.Adapt<AuthenticationResponse>());
            },
            errors => Problem(errors)
        );
    }

    private void SetTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = this._dateTimeProvider.UtcNow.AddDays(7),
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
        };

        this.Response.Cookies.Append(
            "refreshToken",
            refreshToken,
            cookieOptions
        );
    }

    private void RemoveCookie()
    {
        var cookieOptions = new CookieOptions
        {
            Expires = this._dateTimeProvider.UtcNow.AddDays(-1),
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
        };

        this.Response.Cookies.Append(
            "refreshToken",
            string.Empty,
            cookieOptions
        );
    }
}
