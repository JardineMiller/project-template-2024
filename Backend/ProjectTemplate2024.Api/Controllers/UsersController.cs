using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;

namespace ProjectTemplate2024.Api.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet(nameof(Details))]
    public async Task<IActionResult> Details()
    {
        var cmd = new GetUserDetailsQuery();
        var userDetailsResult = await this._mediator.Send(cmd);

        return userDetailsResult.Match(
            success => Ok(success.Adapt<GetUserDetailsResponse>()),
            errors => Problem(errors)
        );
    }
}
