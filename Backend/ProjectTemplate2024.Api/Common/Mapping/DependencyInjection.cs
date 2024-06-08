using System.Reflection;
using Mapster;
using MapsterMapper;

namespace ProjectTemplate2024.Api.Common.Mapping;

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
