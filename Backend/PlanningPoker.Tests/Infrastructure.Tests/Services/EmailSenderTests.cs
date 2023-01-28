using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PlanningPoker.Api.Controllers;
using PlanningPoker.Infrastructure.Email;
using PlanningPoker.Infrastructure.Services;
using Shouldly;
using Xunit;

namespace PlanningPoker.Application.Tests.Infrastructure.Tests.Services;

public class EmailSenderTests
{
    private EmailSender _emailSender = null!;

    [Fact]
    public void SendEmailAsync_WithValidEmail_ShouldSendEmail()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", false, false)
            .AddEnvironmentVariables()
            .AddUserSecrets<ApiController>()
            .Build();

        var emailSettings = config
            .GetSection(EmailSettings.SectionName)
            .Get<EmailSettings>();

        this._emailSender = new EmailSender(
            Options.Create(emailSettings)
        );

        var result = this._emailSender.SendEmail(
            "test@test.com",
            "Test",
            "Test"
        );

        result.ShouldBe(true);
    }

    [Fact]
    public void SendEmailAsync_WithInvalidEmail_ShouldNotSendEmail()
    {
        // Arrange
        var emailSettings = new EmailSettings
        {
            From = "testing@planningpoker.com",
            Host = "smtp.mailtrap.io",
            Port = 2525,
            DisplayName = "Planning Poker - Development",
            Username = "",
            Password = ""
        };

        this._emailSender = new EmailSender(
            Options.Create(emailSettings)
        );

        var result = this._emailSender.SendEmail(
            "test@test.com",
            "Test",
            "Test"
        );

        result.ShouldBe(false);
    }
}
