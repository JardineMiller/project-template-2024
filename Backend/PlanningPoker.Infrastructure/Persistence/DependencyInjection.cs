using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningPoker.Application.Common.Interfaces.Repositories;
using PlanningPoker.Infrastructure.Persistence.Repositories;

namespace PlanningPoker.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var databaseSettings = new DatabaseSettings();
        configuration.Bind(
            DatabaseSettings.SectionName,
            databaseSettings
        );
        services.AddSingleton(Options.Create(databaseSettings));

        services
            .AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        connectionString: databaseSettings.ConnectionString!
                    );
                }
            )
            .ApplyMigrations();

        return services;
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        services.AddTransient<IGameRepository, GameRepository>();
        services.AddTransient<IPlayerRepository, PlayerRepository>();

        return services;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static IServiceCollection ApplyMigrations(
        this IServiceCollection services
    )
    {
        var dbContext = services
            .BuildServiceProvider()
            .GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();

        return services;
    }
}
