using System.Linq.Expressions;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Common.Interfaces.Repositories;

public interface IGameRepository
{
    Task<string> CreateAsync(
        Domain.Entities.Game game,
        CancellationToken cancellationToken
    );

    Task<Domain.Entities.Game?> GetAsync(
        string code,
        CancellationToken cancellationToken,
        params Expression<Func<Domain.Entities.Game, object>>[] includes
    );

    Task<List<Domain.Entities.Game>> GetAllForUserAsync(
        string userId,
        CancellationToken cancellationToken,
        params Expression<Func<Domain.Entities.Game, object>>[] includes
    );

    Task AddPlayerToGameAsync(Player player, Guid gameId);
}
