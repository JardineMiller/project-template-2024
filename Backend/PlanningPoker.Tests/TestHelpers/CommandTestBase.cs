using System;
using PlanningPoker.Infrastructure.Persistence;

namespace PlanningPoker.Application.Tests.TestHelpers;

public class CommandTestBase : IDisposable
{
    public ApplicationDbContext Context { get; set; }

    public CommandTestBase()
    {
        this.Context = TestDbContextFactory.Create();
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(this.Context);
    }
}
