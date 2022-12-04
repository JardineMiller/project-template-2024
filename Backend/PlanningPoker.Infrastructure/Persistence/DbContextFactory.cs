using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PlanningPoker.Infrastructure.Services;

namespace PlanningPoker.Infrastructure.Persistence;

public class DbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly DatabaseSettings _databaseSettings;

    public DbContextFactory(DatabaseSettings settings)
    {
        this._databaseSettings = settings;
    }

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer(
            _databaseSettings?.ConnectionString
        );

        return new ApplicationDbContext(
            optionsBuilder.Options,
            new DateTimeProvider()
        );
    }
}
