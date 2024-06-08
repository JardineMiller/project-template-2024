using ProjectTemplate2024.Api.Hubs;

namespace ProjectTemplate2024.Api.Common.Extensions;

public static class WebApplicationExtensions
{
    private const string prefix = "/hubs";

    public static WebApplication? MapHubs(this WebApplication? app)
    {
        if (app is null)
        {
            return null;
        }

        app.MapHub<GameHub>($"{prefix}/game");

        return app;
    }
}
