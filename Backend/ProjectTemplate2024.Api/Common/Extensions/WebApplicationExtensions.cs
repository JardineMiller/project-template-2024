using PlanningPoker.Api.Hubs;

namespace PlanningPoker.Api.Common.Extensions;

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
