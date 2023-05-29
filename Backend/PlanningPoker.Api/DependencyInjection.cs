using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using PlanningPoker.Api.Common.Errors;
using PlanningPoker.Api.Common.Mapping;
using PlanningPoker.Application.Settings;
using PlanningPoker.Infrastructure.Email;

namespace PlanningPoker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddControllers();

        return services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(
                typeof(PlanningPoker.Application.DependencyInjection).Assembly
            )
            .AddMappings()
            .AddCors(configuration)
            .AddSingleton<
                ProblemDetailsFactory,
                ErrorDetailsFactory
            >();
    }

    private static IServiceCollection AddCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var clientUrl = configuration
            .GetSection(ClientAppSettings.SectionName + ":Url")
            .Value;

        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(clientUrl)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
