using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjectTemplate2024.Api.Common.Errors;
using ProjectTemplate2024.Api.Common.Mapping;
using ProjectTemplate2024.Application.Settings;

namespace ProjectTemplate2024.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddControllers();
        services.AddSignalRCore();

        return services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(
                typeof(Application.DependencyInjection).Assembly
            )
            .AddMappings()
            .AddCors(configuration)
            .AddSingleton<ProblemDetailsFactory, ErrorDetailsFactory>();
    }

    private static IServiceCollection AddCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var clientUrl = configuration
            .GetSection(ClientAppSettings.SectionName + ":Url")
            .Value;

        return services.AddCors(
            options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy
                            .WithOrigins(clientUrl)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            }
        );
    }
}
