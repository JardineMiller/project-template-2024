using MediatR;
using Microsoft.AspNetCore.SignalR;
using ProjectTemplate2024.Application.Common.Interfaces.Services;

namespace ProjectTemplate2024.Api.Hubs;

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
        _mediator = mediator;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task JoinGame(string gameCode, string user)
    {
        var groupTask = Groups.AddToGroupAsync(
            Context.ConnectionId,
            gameCode
        );

        var messageTask = Clients
            .Group(gameCode)
            .PlayerConnected(user, user);

        return Task.WhenAll(groupTask, messageTask);
    }

    public Task LeaveGame(string gameCode, string user)
    {
        var groupTask = Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            gameCode
        );

        var messageTask = Clients
            .Group(gameCode)
            .PlayerDisconnected(user, user);

        return Task.WhenAll(groupTask, messageTask);
    }

    public Task SendMessage(string gameCode, string user, string message)
    {
        return Clients
            .Group(gameCode)
            .ReceiveMessage(
                user,
                message,
                _dateTimeProvider.UtcNow,
                Guid.NewGuid().ToString()
            );
    }

    public Task LikeMessage(string gameCode, string messageId)
    {
        return Clients.Group(gameCode).ReceiveLike(messageId);
    }

    public Task UnlikeMessage(string gameCode, string messageId)
    {
        return Clients.Group(gameCode).ReceiveUnlike(messageId);
    }
}
