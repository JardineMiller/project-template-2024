using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PlanningPoker.Infrastructure.Services;

namespace PlanningPoker.Infrastructure.Persistence;

public class DbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "../ProjectTemplate2024.Api"
                )
            )
            .AddJsonFile("appsettings.Development.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        var databaseSettings = config
            .GetSection(DatabaseSettings.SectionName)
            .Get<DatabaseSettings>();

        var optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer(
            databaseSettings.ConnectionString!
        );

        return new ApplicationDbContext(
            optionsBuilder.Options,
            new DateTimeProvider()
        );
    }
}
