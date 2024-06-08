using System.Linq.Expressions;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(
        string email,
        CancellationToken cancellationToken,
        params Expression<Func<User, object>>[] includes
    );

    Task<User?> GetUserByRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken
    );
}
