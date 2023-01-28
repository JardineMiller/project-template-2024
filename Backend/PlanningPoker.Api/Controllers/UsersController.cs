using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Application.Game.Queries.GetUserGames;
using PlanningPoker.Contracts.Game;

namespace PlanningPoker.Api.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("{userId}/games")]
    public async Task<IActionResult> GetUserGames(
        [FromRoute] string userId
    )
    {
        var request = new GetUserGamesQuery(userId);
        var result = await this._mediator.Send(request);

        return result.Match(
            success => Ok(success.Adapt<GetUserGamesResponse>()),
            errors => Problem(errors)
        );
    }
}
