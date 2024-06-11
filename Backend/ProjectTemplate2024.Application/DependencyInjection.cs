using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Application.PipelineBehaviours;
using ProjectTemplate2024.Application.Services;
using ProjectTemplate2024.Application.Settings;

namespace ProjectTemplate2024.Application;

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

        services.AddTransient<IBlobStorageService, BlobStorageService>();

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
