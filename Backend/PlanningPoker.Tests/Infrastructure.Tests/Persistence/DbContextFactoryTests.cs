using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanningPoker.Infrastructure.Persistence;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Infrastructure.Tests.Persistence;

public class DbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_WithConnectionString_ReturnsDbContext()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        var databaseSettings = new DatabaseSettings();

        config
            .GetSection(DatabaseSettings.SectionName)
            .Bind(databaseSettings);

        var factory = new DbContextFactory(databaseSettings);

        // Act
        using var context = factory.CreateDbContext(
            Array.Empty<string>()
        );

        // Assert
        context.Database.EnsureCreated();
        context.Database
            .GetConnectionString()
            .ShouldBe(databaseSettings.ConnectionString);
    }
}
