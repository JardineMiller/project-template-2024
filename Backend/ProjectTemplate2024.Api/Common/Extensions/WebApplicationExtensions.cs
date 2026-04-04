using ProjectTemplate2024.Api.Hubs;

namespace ProjectTemplate2024.Api.Common.Extensions;

public static class WebApplicationExtensions
{
    private const string Prefix = "/hubs";

    public static WebApplication? MapHubs(this WebApplication? app)
    {
        if (app is null)
        {
            return null;
        }

        app.MapHub<GameHub>($"{Prefix}/game");

        return app;
    }
}
