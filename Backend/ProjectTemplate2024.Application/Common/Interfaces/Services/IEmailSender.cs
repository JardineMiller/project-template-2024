namespace ProjectTemplate2024.Application.Common.Interfaces.Services;

public interface IEmailSender
{
    bool SendEmail(string toEmail, string subject, string body);
}
