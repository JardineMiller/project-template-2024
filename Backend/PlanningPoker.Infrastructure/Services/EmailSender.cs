using System.Net;
using System.Net.Mail;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Infrastructure.Email;

namespace PlanningPoker.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(EmailSettings emailSettings)
    {
        this._emailSettings = emailSettings;
    }

    public bool SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var client = new SmtpClient(
                this._emailSettings.Host,
                this._emailSettings.Port
            )
            {
                Credentials = new NetworkCredential(
                    this._emailSettings.Username,
                    this._emailSettings.Password
                ),
                EnableSsl = true
            };

            var message = new MailMessage(
                this._emailSettings.From,
                toEmail,
                subject,
                body
            );

            message.IsBodyHtml = true;

            client.Send(message);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
