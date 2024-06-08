using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTemplate2024.Application.Common.Interfaces.Generators;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Entities;
using ProjectTemplate2024.Infrastructure.Authentication;
using ProjectTemplate2024.Infrastructure.Email;
using ProjectTemplate2024.Infrastructure.Generators;
using ProjectTemplate2024.Infrastructure.Persistence;
using ProjectTemplate2024.Infrastructure.Services;

namespace ProjectTemplate2024.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddTransient<ITinyGuidGenerator, TinyGuidGenerator>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddAuth(configuration)
            .AddDatabase(configuration)
            .AddRepositories()
            .AddEmail(configuration)
            .AddIdentity();

        return services;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static IServiceCollection AddIdentity(
        this IServiceCollection services
    )
    {
        services
            .AddIdentity<User, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredUniqueChars = 0;

                    options.SignIn.RequireConfirmedEmail = true;
                }
            )
            .AddTokenProvider<DataProtectorTokenProvider<User>>(
                TokenOptions.DefaultProvider
            )
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
