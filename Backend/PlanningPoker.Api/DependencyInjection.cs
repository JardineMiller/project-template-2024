using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PlanningPoker.Api.Common.Errors;
using PlanningPoker.Api.Common.Mapping;

namespace PlanningPoker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services
    )
    {
        services.AddControllers();

        return services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly()
            )
            .AddMappings()
            .AddSingleton<
                ProblemDetailsFactory,
                ErrorDetailsFactory
            >();
    }
}
