using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Infrastructure.Persistence.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;

    public GameRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<string> CreateAsync(
        Game game,
        CancellationToken cancellationToken
    )
    {
        await this._context.AddAsync(game, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);

        return game.Code;
    }
}
