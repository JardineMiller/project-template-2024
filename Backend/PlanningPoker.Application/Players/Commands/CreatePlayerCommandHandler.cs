using ErrorOr;
using MediatR;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Players.Commands;

public class CreatePlayerCommandHandler
    : IRequestHandler<CreatePlayerCommand, ErrorOr<CreatePlayerResult>>
{
    private readonly IPlayerRepository _playerRepository;

    public CreatePlayerCommandHandler(IPlayerRepository playerRepository)
    {
        this._playerRepository = playerRepository;
    }

    public async Task<ErrorOr<CreatePlayerResult>> Handle(
        CreatePlayerCommand request,
        CancellationToken cancellationToken
    )
    {
        if (
            await this._playerRepository.GetByUserIdAsync(
                request.UserId,
                cancellationToken
            )
            is not null
        )
        {
            return Errors.Common.Duplicate(nameof(Player));
        }

        var player = new Player
        {
            UserId = request.UserId,
            DisplayName = request.DisplayName
        };

        var playerId = await this._playerRepository.CreateAsync(
            player,
            cancellationToken
        );

        return new CreatePlayerResult(playerId);
    }
}
