using System.IO;
using Microsoft.Extensions.Options;
using Moq;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Application.Settings;
using ProjectTemplate2024.Infrastructure.Services;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Infrastructure.Tests.Services;

public class EmailServiceTests
{
    private readonly IEmailService _emailService;
    private readonly Mock<IEmailSender> _emailSenderMock = new();
    private readonly Mock<IEmailPathService> _emailPathServiceMock = new();

    private readonly ClientAppSettings _clientAppSettings = new()
    {
        Url = "fake-url",
    };

    public EmailServiceTests()
    {
        _emailService = new EmailService(
            _emailSenderMock.Object,
            _emailPathServiceMock.Object,
            Options.Create(_clientAppSettings)
        );
    }

    [Fact]
    public void SendConfirmationEmail_WithValidEmail_ShouldSendEmail()
    {
        _emailPathServiceMock
            .Setup(x => x.GetEmailPath("EmailConfirmation.html"))
            .Returns(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Email",
                    "Templates",
                    "EmailConfirmation.html"
                )
            );

        var toEmail = "to";
        var firstName = "first name";
        var token = "token";

        _emailService.SendConfirmationEmail(toEmail, firstName, token);

        _emailSenderMock.Verify(
            x =>
                x.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
            Times.Once
        );
    }

    [Fact]
    public void SendPasswordReset_WithValidEmail_ShouldSendEmail()
    {
        _emailPathServiceMock
            .Setup(x => x.GetEmailPath("ResetPassword.html"))
            .Returns(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Email",
                    "Templates",
                    "ResetPassword.html"
                )
            );

        var toEmail = "to";
        var token = "token";

        _emailService.SendPasswordResetEmail(toEmail, token);

        _emailSenderMock.Verify(
            x =>
                x.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
            Times.Once
        );
    }
}
