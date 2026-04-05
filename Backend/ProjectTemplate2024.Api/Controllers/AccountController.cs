using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate2024.Api.Common.Mapping;
using ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;
using ProjectTemplate2024.Application.Account.Commands.ResetPassword;
using ProjectTemplate2024.Contracts.Account.RequestResetPassword;
using ProjectTemplate2024.Contracts.Account.ResetPassword;

namespace ProjectTemplate2024.Api.Controllers;

public class AccountController : ApiController
{
    private readonly ISender _mediator;

    public AccountController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var cmd = request.ToCommand();
        var result = await _mediator.Send(cmd);

        return result.Match(
            success => Ok(success.ToResponse()),
            errors => Problem(errors)
        );
    }

    [AllowAnonymous]
    [HttpGet(nameof(RequestResetPassword))]
    public async Task<IActionResult> RequestResetPassword(
        RequestResetPasswordRequest request
    )
    {
        var cmd = request.ToCommand();
        var result = await _mediator.Send(cmd);

        return result.Match(
            success => Ok(success.ToResponse()),
            errors => Problem(errors)
        );
    }
}
