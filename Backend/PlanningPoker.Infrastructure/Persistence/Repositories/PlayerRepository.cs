using Microsoft.EntityFrameworkCore;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Infrastructure.Persistence.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<string> CreateAsync(
        Player player,
        CancellationToken cancellationToken
    )
    {
        await this._context.AddAsync(player, cancellationToken);
        await this._context.SaveChangesAsync(cancellationToken);

        return player.Id;
    }

    public async Task<Player?> GetAsync(
        string id,
        CancellationToken cancellationToken
    )
    {
        return await this._context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
