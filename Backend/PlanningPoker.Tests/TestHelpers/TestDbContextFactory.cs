using System;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Domain.Entities;
using PlanningPoker.Infrastructure.Persistence;
using PlanningPoker.Infrastructure.Services;

namespace PlanningPoker.Application.Tests.TestHelpers;

public class TestDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        var context = new ApplicationDbContext(
            options,
            new DateTimeProvider()
        );

        context.Database.EnsureCreated();

        AddUsers(context);
        AddGames(context);

        context.SaveChanges();
        return context;
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();

        context.Dispose();
    }

    private static void AddUsers(ApplicationDbContext context)
    {
        var user1 = new User
        {
            Id = "0001",
            UserName = "User 1",
            Email = "user1@test.com",
            EmailConfirmed = true,
            FirstName = "First Name 1",
            LastName = "Last Name 1"
        };

        var user2 = new User
        {
            Id = "0002",
            UserName = "User 2",
            Email = "user2@test.com",
            EmailConfirmed = true,
            FirstName = "First Name 1",
            LastName = "Last Name 2"
        };

        context.Users.AddRange(user1, user2);
    }

    private static void AddGames(ApplicationDbContext context)
    {
        var game1 = new Domain.Entities.Game
        {
            Name = "Game 1",
            OwnerId = "0001",
            Code = "1234567A"
        };

        var game3 = new Domain.Entities.Game
        {
            Name = "Game 3",
            OwnerId = "0001",
            Code = "1234567C"
        };

        var game2 = new Domain.Entities.Game
        {
            Name = "Game 2",
            Description = "Game 2 - Description",
            OwnerId = "0002",
            Code = "1234567B"
        };

        context.Games.AddRange(game1, game2, game3);
    }
}
