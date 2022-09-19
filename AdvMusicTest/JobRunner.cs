using AdvMusicTest.Scripts;
using Hangfire;
using Hangfire.Dashboard;

namespace AdvMusicTest;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // Allow all authenticated users to see the Dashboard (potentially dangerous).
        return true;
    }
}

public class JobRunner
{
    protected ILogger _logger;
    protected LitResParser _parser;

    public JobRunner(ILogger logger, LitResParser parser)
    {
        _logger = logger;
        _parser = parser;
    }

    public async Task Run(WebApplication app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] {new MyAuthorizationFilter()}
            });
        });

        RecurringJob.AddOrUpdate("LitResParser", () => _parser.Download(), Cron.Daily(1, 1));

        app.Run();
    }
}