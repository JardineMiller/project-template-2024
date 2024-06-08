using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Game.Commands.Create;

public record CreateGameCommand(
    string Name,
    string? Description,
    string OwnerId
) : IRequest<ErrorOr<CreateGameResult>>;
