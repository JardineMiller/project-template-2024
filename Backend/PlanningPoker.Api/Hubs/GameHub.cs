using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Application.Game.Queries.JoinGame;
using PlanningPoker.Application.Players.Queries.GetPlayer;

namespace PlanningPoker.Api.Hubs;

public interface IGameHub
{
    Task PlayerConnected(
        string playerName,
        string playerId,
        CancellationToken cancellationToken
    );

    Task PlayerDisconnected(
        string playerName,
        string playerId,
        CancellationToken cancellationToken
    );
}

public class GameHub : Hub<IGameHub>
{
    private readonly ISender _mediator;

    public GameHub(ISender mediator)
    {
        this._mediator = mediator;
    }

    public async Task ConnectToGame(
        string gameCode,
        string playerId,
        CancellationToken cancellationToken = new()
    )
    {
        Task OnSuccess(JoinGameResult success)
        {
            var groupTask = this.Groups.AddToGroupAsync(
                this.Context.ConnectionId,
                success.GameCode,
                cancellationToken
            );

            var messageTask = this.Clients
                .Group(gameCode)
                .PlayerConnected(
                    success.PlayerName,
                    success.PlayerId,
                    cancellationToken
                );

            return Task.WhenAll(groupTask, messageTask);
        }

        Task OnError(List<Error> errors)
        {
            return Task.CompletedTask;
        }

        var qry = new JoinGameQuery(gameCode, playerId);
        var result = await this._mediator.Send(qry, cancellationToken);

        await result.Match(
            success => OnSuccess(success),
            errors => OnError(errors)
        );
    }

    public async Task DisconnectFromGame(
        string gameCode,
        string playerId,
        CancellationToken cancellationToken = new()
    )
    {
        Task OnSuccess(GetPlayerResult success)
        {
            var messageTask = this.Clients
                .Group(gameCode)
                .PlayerDisconnected(
                    success.PlayerName,
                    success.PlayerId,
                    cancellationToken
                );

            return messageTask;
        }

        Task OnError(List<Error> errors)
        {
            return Task.CompletedTask;
        }

        var qry = new GetPlayerQuery(playerId);
        var result = await this._mediator.Send(qry, cancellationToken);

        await result.Match(
            success => OnSuccess(success),
            errors => OnError(errors)
        );
    }
}
