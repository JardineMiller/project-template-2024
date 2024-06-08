using System.Web;
using Microsoft.Extensions.Options;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Application.Settings;

namespace ProjectTemplate2024.Infrastructure.Services;

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
            $"Welcome to ProjectTemplate2024, {firstName}",
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
            "ProjectTemplate2024 - Reset Password",
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
