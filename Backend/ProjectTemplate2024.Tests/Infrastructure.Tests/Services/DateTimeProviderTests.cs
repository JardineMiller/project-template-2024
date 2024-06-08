﻿using System;
using PlanningPoker.Infrastructure.Services;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Infrastructure.Tests.Services;

public class DateTimeProviderTests
{
    private readonly DateTimeProvider _dateTimeProvider = new();

    [Fact]
    public void Provider_Returns_Current_DateTime()
    {
        var now = this._dateTimeProvider.UtcNow;

        now.ShouldNotBe(DateTime.MinValue);
        now.ShouldNotBe(DateTime.MaxValue);
    }

    [Fact]
    public void Provider_Returns_Current_DateTimeOffset()
    {
        var now = this._dateTimeProvider.Now;

        now.ShouldNotBe(DateTimeOffset.MinValue);
        now.ShouldNotBe(DateTimeOffset.MaxValue);
    }
}
