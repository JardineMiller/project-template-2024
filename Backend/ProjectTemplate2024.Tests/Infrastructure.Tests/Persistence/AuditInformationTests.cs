using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Entities;
using ProjectTemplate2024.Infrastructure.Persistence;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Infrastructure.Tests.Persistence;

public class AuditInformationTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();

    private readonly User _user =
        new()
        {
            Id = "0001",
            UserName = "User 1",
            Email = "user1@test.com",
            EmailConfirmed = true,
            DisplayName = "Display Name",
        };

    public AuditInformationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        this._context = new ApplicationDbContext(
            options,
            this._dateTimeProviderMock.Object
        );

        this._context.Users.Add(this._user);
        this._context.SaveChanges();
    }

    [Fact]
    public void IAuditable_HasModifiedPropertiesSet_WhenModified()
    {
        var now = DateTimeOffset.Now.UtcDateTime;
        this._dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        var user1 = this._context.Users.FirstOrDefault(
            x => x.Id == this._user.Id
        )!;

        user1.ModifiedOn.ShouldBe(default);

        this._context.Users.Update(user1);
        this._context.SaveChanges();

        user1.ModifiedOn.ShouldBe(now);
    }

    [Fact]
    public void IAuditable_HasAuditablePropertiesSet_WhenCreated()
    {
        var now = DateTimeOffset.Now.UtcDateTime;
        this._dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        var user2 = new User
        {
            Id = "0002",
            UserName = "User 2",
            Email = "user2@test.com",
            EmailConfirmed = true,
            DisplayName = "First Name",
        };

        user2.CreatedOn.ShouldBe(default);
        user2.ModifiedOn.ShouldBe(default);

        this._context.Users.Add(user2);
        this._context.SaveChanges();

        user2.CreatedOn.ShouldBe(now);
        user2.ModifiedOn.ShouldBe(default);
    }

    [Fact]
    public void IAuditable_HasModifiedPropertiesSet_WhenModified_Async()
    {
        var now = DateTimeOffset.Now.UtcDateTime;
        this._dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        var user1 = this._context.Users.FirstOrDefault(
            x => x.Id == this._user.Id
        )!;

        user1.ModifiedOn.ShouldBe(default);

        this._context.Users.Update(user1);
        this._context.SaveChangesAsync();

        user1.ModifiedOn.ShouldBe(now);
    }

    [Fact]
    public void IAuditable_HasAuditablePropertiesSet_WhenCreated_Async()
    {
        var now = DateTimeOffset.Now.UtcDateTime;
        this._dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        var user2 = new User
        {
            Id = "0002",
            UserName = "User 2",
            Email = "user2@test.com",
            EmailConfirmed = true
        };

        user2.CreatedOn.ShouldBe(default);
        user2.ModifiedOn.ShouldBe(default);

        this._context.Users.Add(user2);
        this._context.SaveChangesAsync();

        user2.CreatedOn.ShouldBe(now);
        user2.ModifiedOn.ShouldBe(default);
    }
}
