using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PlanningPoker.Application.Common.Interfaces.Generators;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Game.Commands.Create;

public class CreateGameCommandHandler
    : IRequestHandler<CreateGameCommand, ErrorOr<CreateGameResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly IGameRepository _gameRepository;
    private readonly ITinyGuidGenerator _guidGenerator;

    public CreateGameCommandHandler(
        UserManager<User> userManager,
        IGameRepository gameRepository,
        ITinyGuidGenerator guidGenerator
    )
    {
        this._userManager = userManager;
        this._gameRepository = gameRepository;
        this._guidGenerator = guidGenerator;
    }

    public async Task<ErrorOr<CreateGameResult>> Handle(
        CreateGameCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await this._userManager.FindByIdAsync(
            request.OwnerId
        );

        if (user is null)
        {
            return Errors.Common.NotFound(nameof(User));
        }

        var game = new Domain.Entities.Game
        {
            Name = request.Name,
            Description = request.Description,
            Code = this._guidGenerator.Generate(),
            OwnerId = request.OwnerId
        };

        var code = await this._gameRepository.CreateAsync(
            game,
            cancellationToken
        );

        return new CreateGameResult(code);
    }
}
