using ErrorOr;
using MediatR;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Game.Queries.JoinGame;

public class JoinGameQueryHandler
    : IRequestHandler<JoinGameQuery, ErrorOr<JoinGameResult>>
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;

    public JoinGameQueryHandler(
        IGameRepository gameRepository,
        IPlayerRepository playerRepository
    )
    {
        this._gameRepository = gameRepository;
        this._playerRepository = playerRepository;
    }

    public async Task<ErrorOr<JoinGameResult>> Handle(
        JoinGameQuery request,
        CancellationToken cancellationToken
    )
    {
        var player = await this._playerRepository.GetAsync(
            request.PlayerId,
            cancellationToken
        );

        if (player is null)
        {
            return Errors.Common.NotFound(nameof(Player));
        }

        var game = await this._gameRepository.GetAsync(
            request.GameCode,
            cancellationToken,
            g => g.Players
        );

        if (game is null)
        {
            return Errors.Common.NotFound(
                nameof(Domain.Entities.Game)
            );
        }

        if (
            game.Players.FirstOrDefault(x => x.Id == player.Id)
            is null
        )
        {
            await this._gameRepository.AddPlayerToGameAsync(
                player,
                game.Id
            );
        }

        return new JoinGameResult(player.Name, player.Id, game.Code);
    }
}
