﻿namespace PlanningPoker.Infrastructure.Email;

public record EmailSettings
{
    public static string SectionName = "MailSettings";
    public string From { get; init; } = null!;
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string DisplayName { get; init; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
