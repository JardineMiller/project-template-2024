﻿using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Application.Game.Commands.Create;
using PlanningPoker.Contracts.Game;

namespace PlanningPoker.Api.Controllers;

public class GameController : ApiController
{
    private readonly ISender _mediator;

    public GameController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet(nameof(All))]
    public IActionResult All()
    {
        return Ok();
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
}
