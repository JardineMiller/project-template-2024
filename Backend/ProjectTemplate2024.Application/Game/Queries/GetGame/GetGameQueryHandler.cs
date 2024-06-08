using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Domain.Common.Errors;

namespace ProjectTemplate2024.Application.Game.Queries.GetGame;

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

        return GetGameResult.From.Game(game);
    }
}
