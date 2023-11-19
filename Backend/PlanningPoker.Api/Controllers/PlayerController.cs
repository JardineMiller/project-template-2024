using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Application.Players.Commands;
using PlanningPoker.Contracts.Player.CreatePlayer;

namespace PlanningPoker.Api.Controllers;

public class PlayerController : ApiController
{
    private readonly ISender _mediator;

    public PlayerController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreatePlayerRequest request)
    {
        var cmd = request.Adapt<CreatePlayerCommand>();
        var result = await this._mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<CreatePlayerResponse>()),
            errors => Problem(errors)
        );
    }
}
