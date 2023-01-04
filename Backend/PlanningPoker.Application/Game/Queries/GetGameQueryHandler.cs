using ErrorOr;
using MediatR;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Common.Errors;

namespace PlanningPoker.Application.Game.Queries;

public class GetGameQueryHandler
    : IRequestHandler<GetGameQuery, ErrorOr<GetGameResult>>
{
    private readonly IGameRepository _gameRepository;

    public GetGameQueryHandler(IGameRepository gameRepository)
    {
        this._gameRepository = gameRepository;
    }

    public async Task<ErrorOr<GetGameResult>> Handle(
        GetGameQuery request,
        CancellationToken cancellationToken
    )
    {
        var game = await this._gameRepository.GetAsync(
            request.Code,
            cancellationToken
        );

        if (game is null)
        {
            return Errors.Common.NotFound(nameof(Game));
        }

        return new GetGameResult(
            game.Name,
            game.Description,
            game.Code,
            game.OwnerId,
            game.Owner
        );
    }
}
