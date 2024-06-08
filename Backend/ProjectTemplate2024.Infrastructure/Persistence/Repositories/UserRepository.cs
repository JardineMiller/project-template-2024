using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<User?> GetUserByEmail(
        string email,
        CancellationToken cancellationToken = new(),
        params Expression<Func<User, object>>[] includes
    )
    {
        var query = this._userManager.Users.Where(x => x.Email == email);

        query = includes.Aggregate(
            query.AsQueryable(),
            (current, include) => current.Include(include)
        );

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetUserByRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken = new()
    )
    {
        return await this._userManager.Users.Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(
                x => x.RefreshTokens.Any(t => t.Token == refreshToken),
                cancellationToken
            );
    }
}
