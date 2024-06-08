using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Application.Game.Queries.GetGame;
using PlanningPoker.Contracts.Game.CreateGame;
using PlanningPoker.Contracts.Game.GetGame;

namespace PlanningPoker.Api.Controllers;

public class GameController : ApiController
{
    private readonly ISender _mediator;

    public GameController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreateGameRequest request)
    {
        var cmd = request.Adapt<CreateGameCommand>();
        var result = await this._mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<CreateGameResponse>()),
            errors => Problem(errors)
        );
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(GetGameRequest request)
    {
        var query = request.Adapt<GetGameQuery>();
        var result = await this._mediator.Send(query);

        return result.Match(
            success => Ok(success.Adapt<GetGameResponse>()),
            errors => Problem(errors)
        );
    }
}
