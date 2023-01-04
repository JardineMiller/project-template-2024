namespace PlanningPoker.Application.Common.Interfaces.Repositories;

public interface IGameRepository
{
    Task<string> CreateAsync(
        Domain.Entities.Game game,
        CancellationToken cancellationToken
    );

    Task<Domain.Entities.Game?> GetAsync(
        string code,
        CancellationToken cancellationToken
    );
}
