using System;
using Microsoft.EntityFrameworkCore;
using ProjectTemplate2024.Domain.Entities;
using ProjectTemplate2024.Infrastructure.Persistence;
using ProjectTemplate2024.Infrastructure.Services;

namespace ProjectTemplate2024.Application.Tests.TestHelpers;

public class TestDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options, new DateTimeProvider());

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
            EmailConfirmed = true,
            DisplayName = "Display Name 1",
        };

        var user2 = new User
        {
            Id = "0002",
            UserName = "User 2",
            Email = "user2@test.com",
            EmailConfirmed = true,
            DisplayName = "Display Name 2",
        };

        context.Users.AddRange(user1, user2);
    }
}
