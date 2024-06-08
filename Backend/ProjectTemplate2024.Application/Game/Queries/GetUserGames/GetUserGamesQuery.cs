using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Game.Queries.GetUserGames;

public record GetUserGamesQuery(string UserId)
    : IRequest<ErrorOr<GetUserGamesResult>>;
