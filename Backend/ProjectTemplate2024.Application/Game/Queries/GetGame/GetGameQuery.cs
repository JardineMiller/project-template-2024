using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Game.Queries.GetGame;

public record GetGameQuery(string Code)
    : IRequest<ErrorOr<GetGameResult>>;
