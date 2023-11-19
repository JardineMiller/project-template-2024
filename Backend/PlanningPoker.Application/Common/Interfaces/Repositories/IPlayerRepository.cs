using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Common.Interfaces.Repositories;

public interface IPlayerRepository
{
    Task<string> CreateAsync(
        Player player,
        CancellationToken cancellationToken
    );

    Task<Player?> GetAsync(string id, CancellationToken cancellationToken);

    Task<Player?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken
    );
}
