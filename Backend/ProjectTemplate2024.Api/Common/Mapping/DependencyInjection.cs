using System.Reflection;
using Mapster;
using MapsterMapper;

namespace PlanningPoker.Api.Common.Mapping;

public static class DependencyInjection
{
    public static IServiceCollection AddMappings(
        this IServiceCollection services
    )
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetAssembly(typeof(DependencyInjection)));

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
