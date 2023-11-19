using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public async Task AddPlayerToGameAsync(Player player, string gameId)
    {
        // var game = await this._context.Games
        //     .Include(x => x.Players)
        //     .FirstOrDefaultAsync(x => x.Id == gameId);
        //
        // if (
        //     game is null
        //     || game.Players.FirstOrDefault(x => x.Id == player.Id)
        //         is not null
        // )
        // {
        //     return;
        // }
        //
        // game.Players.Add(player);
        // await this._context.SaveChangesAsync();
    }
}
