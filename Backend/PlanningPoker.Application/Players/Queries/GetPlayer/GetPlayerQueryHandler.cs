using ErrorOr;
using MediatR;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Players.Queries.GetPlayer;

public class GetPlayerQueryHandler
    : IRequestHandler<GetPlayerQuery, ErrorOr<GetPlayerResult>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerQueryHandler(IPlayerRepository playerRepository)
    {
        this._playerRepository = playerRepository;
    }

    public async Task<ErrorOr<GetPlayerResult>> Handle(
        GetPlayerQuery request,
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

        return new GetPlayerResult(player.Name, player.Id);
    }
}
