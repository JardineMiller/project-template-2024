using System;
using ProjectTemplate2024.Infrastructure.Persistence;

namespace ProjectTemplate2024.Application.Tests.TestHelpers;

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
