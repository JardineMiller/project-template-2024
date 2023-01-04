using System;
using System.Linq;

namespace PlanningPoker.Application.Tests.TestHelpers;

public static class TestDataGenerator
{
    public static string GenerateRandomString(int length = 6)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var result = new string(
            Enumerable
                .Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray()
        );

        return result;
    }
}
