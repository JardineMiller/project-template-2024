using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Game.Queries.GetUserGames;

public class GetUserGamesQueryHandler
    : IRequestHandler<GetUserGamesQuery, ErrorOr<GetUserGamesResult>>
{
    private readonly IGameRepository _gameRepository;
    private readonly UserManager<User> _userManager;

    public GetUserGamesQueryHandler(
        IGameRepository gameRepository,
        UserManager<User> userManager
    )
    {
        this._gameRepository = gameRepository;
        this._userManager = userManager;
    }

    public async Task<ErrorOr<GetUserGamesResult>> Handle(
        GetUserGamesQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await this._userManager.FindByIdAsync(
            request.UserId
        );

        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var games = await this._gameRepository.GetAllForUserAsync(
            request.UserId,
            cancellationToken: cancellationToken
        );

        return new GetUserGamesResult(request.UserId, games);
    }
}
