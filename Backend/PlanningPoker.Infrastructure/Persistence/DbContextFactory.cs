using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PlanningPoker.Infrastructure.Services;

namespace PlanningPoker.Infrastructure.Persistence;

public class DbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public const string ConnectionString =
        "Server=localhost;Database=PlanningPoker.Dev;Trusted_Connection=True;MultipleActiveResultSets=true"; // TODO Get this from AppSettings

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(ConnectionString);

        return new ApplicationDbContext(
            optionsBuilder.Options,
            new DateTimeProvider()
        );
    }
}
