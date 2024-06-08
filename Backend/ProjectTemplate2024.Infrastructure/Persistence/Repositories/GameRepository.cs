using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Infrastructure.Persistence.Repositories;

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

    public async Task<Game?> GetAsync(
        string code,
        CancellationToken cancellationToken,
        params Expression<Func<Game, object>>[] includes
    )
    {
        var query = this._context.Games.AsNoTracking();

        query = includes.Aggregate(
            query.AsQueryable(),
            (current, include) => current.Include(include)
        );

        return await query.FirstOrDefaultAsync(
            x => x.Code == code,
            cancellationToken
        );
    }

    public async Task<List<Game>> GetAllForUserAsync(
        string userId,
        CancellationToken cancellationToken,
        params Expression<Func<Game, object>>[] includes
    )
    {
        var query = this._context.Games
            .AsNoTracking()
            .Where(x => x.OwnerId == userId);

        query = includes.Aggregate(
            query.AsQueryable(),
            (current, include) => current.Include(include)
        );

        return await query.ToListAsync(cancellationToken);
    }
}
