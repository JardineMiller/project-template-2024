using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Infrastructure.Email;

namespace ProjectTemplate2024.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public bool SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var client = new SmtpClient(
                _emailSettings.Host,
                _emailSettings.Port
            )
            {
                Credentials = new NetworkCredential(
                    _emailSettings.Username,
                    _emailSettings.Password
                ),
                EnableSsl = true
            };

            var message = new MailMessage(
                _emailSettings.From,
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
