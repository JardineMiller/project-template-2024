using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Infrastructure.Services;

namespace PlanningPoker.Infrastructure.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmail(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var emailSettings = new EmailSettings();
        configuration.Bind(EmailSettings.SectionName, emailSettings);
        services.AddSingleton(Options.Create(emailSettings));

        services.AddTransient<IEmailPathService, EmailPathService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
