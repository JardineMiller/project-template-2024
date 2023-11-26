using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Application.Game.Queries.JoinGame;
using PlanningPoker.Application.Players.Queries.GetPlayer;

namespace PlanningPoker.Api.Hubs;

public interface IGameHub
{
    Task PlayerConnected(string playerName, string playerId);

    Task PlayerDisconnected(string playerName, string playerId);

    Task ReceiveMessage(
        string user,
        string message,
        DateTimeOffset timestamp,
        string messageId
    );

    Task ReceiveLike(string messageId);

    Task ReceiveUnlike(string messageId);
}

public class GameHub : Hub<IGameHub>
{
    private readonly ISender _mediator;
    private readonly ILogger _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GameHub(
        ISender mediator,
        ILogger<GameHub> logger,
        IDateTimeProvider dateTimeProvider
    )
    {
        this._mediator = mediator;
        this._logger = logger;
        this._dateTimeProvider = dateTimeProvider;
    }

    public Task JoinGame(string gameCode, string user)
    {
        var groupTask = this.Groups.AddToGroupAsync(
            this.Context.ConnectionId,
            gameCode
        );

        var messageTask = this.Clients
            .Group(gameCode)
            .PlayerConnected(user, user);

        return Task.WhenAll(groupTask, messageTask);
    }

    public Task LeaveGame(string gameCode, string user)
    {
        var groupTask = this.Groups.RemoveFromGroupAsync(
            this.Context.ConnectionId,
            gameCode
        );

        var messageTask = this.Clients
            .Group(gameCode)
            .PlayerDisconnected(user, user);

        return Task.WhenAll(groupTask, messageTask);
    }

    public Task SendMessage(string gameCode, string user, string message)
    {
        return this.Clients
            .Group(gameCode)
            .ReceiveMessage(
                user,
                message,
                this._dateTimeProvider.UtcNow,
                Guid.NewGuid().ToString()
            );
    }

    public Task LikeMessage(string gameCode, string messageId)
    {
        return this.Clients.Group(gameCode).ReceiveLike(messageId);
    }

    public Task UnlikeMessage(string gameCode, string messageId)
    {
        return this.Clients.Group(gameCode).ReceiveUnlike(messageId);
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
                success.GameCode
            );

            var messageTask = this.Clients
                .Group(gameCode)
                .PlayerConnected(success.PlayerName, success.PlayerId);

            return Task.WhenAll(groupTask, messageTask);
        }

        Task OnError(List<Error> errors)
        {
            return Task.CompletedTask;
        }

        var qry = new JoinGameQuery(gameCode, playerId);
        var result = await this._mediator.Send(qry);

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
                .PlayerDisconnected(success.PlayerName, success.PlayerId);

            return messageTask;
        }

        Task OnError(List<Error> errors)
        {
            return Task.CompletedTask;
        }

        var qry = new GetPlayerQuery(playerId);
        var result = await this._mediator.Send(qry);

        await result.Match(
            success => OnSuccess(success),
            errors => OnError(errors)
        );
    }
}
