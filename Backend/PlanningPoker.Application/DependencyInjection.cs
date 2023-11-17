using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningPoker.Application.PipelineBehaviours;
using PlanningPoker.Application.Settings;

namespace PlanningPoker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        var clientAppSettings = new ClientAppSettings();
        configuration.Bind(ClientAppSettings.SectionName, clientAppSettings);

        services.AddSingleton(Options.Create(clientAppSettings));

        services
            .AddMediatR(typeof(DependencyInjection).Assembly)
            .AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(RequestValidationBehaviour<,>)
            )
            .AddSignalR();

        return services;
    }
}
