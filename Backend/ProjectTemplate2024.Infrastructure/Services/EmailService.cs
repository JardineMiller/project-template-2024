using System.Web;
using Microsoft.Extensions.Options;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Application.Settings;

namespace PlanningPoker.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailPathService _emailPathService;
    private readonly ClientAppSettings _clientAppSettings;

    public EmailService(
        IEmailSender emailSender,
        IEmailPathService emailPathService,
        IOptions<ClientAppSettings> clientAppSettings
    )
    {
        this._emailSender = emailSender;
        this._emailPathService = emailPathService;
        this._clientAppSettings = clientAppSettings.Value;
    }

    public void SendConfirmationEmail(
        string toEmail,
        string firstName,
        string token
    )
    {
        var mailText = GetEmailContent("EmailConfirmation.html");
        var encodedToken = HttpUtility.UrlEncode(token);

        mailText = mailText
            .Replace("[username]", firstName)
            .Replace("[email]", toEmail)
            .Replace(
                "[welcome-link]",
                $"{this._clientAppSettings.Url}/confirm?token={encodedToken}&email={toEmail}"
            );

        this._emailSender.SendEmail(
            toEmail,
            $"Welcome to Planning Poker, {firstName}",
            mailText
        );
    }

    public void SendPasswordResetEmail(string toEmail, string token)
    {
        var mailText = GetEmailContent("ResetPassword.html");
        var encodedToken = HttpUtility.UrlEncode(token);

        mailText = mailText
            .Replace("[email]", toEmail)
            .Replace(
                "[reset-link]",
                $"https://localhost:7097/api/account/resetpassword?token={encodedToken}&email={toEmail}" //TODO Put url in AppSettings?
            );

        this._emailSender.SendEmail(
            toEmail,
            "Planning Poker - Reset Password",
            mailText
        );
    }

    private string GetEmailContent(string emailFileName)
    {
        var streamReader = new StreamReader(
            this._emailPathService.GetEmailPath(emailFileName)
        );

        var mailText = streamReader.ReadToEnd();
        streamReader.Close();

        return mailText;
    }
}
