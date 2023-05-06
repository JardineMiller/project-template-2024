using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Application.PipelineBehaviours;

namespace PlanningPoker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
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
