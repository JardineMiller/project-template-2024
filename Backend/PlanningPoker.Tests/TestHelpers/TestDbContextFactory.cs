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
            EmailConfirmed = true
        };

        var user2 = new User
        {
            Id = "0002",
            UserName = "User 2",
            Email = "user2@test.com",
            EmailConfirmed = true
        };

        context.Users.AddRange(user1, user2);
    }
}
