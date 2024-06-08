using System.Linq.Expressions;

namespace ProjectTemplate2024.Application.Common.Interfaces.Repositories;

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
}
