using AdvMusicTest;
using AdvMusicTest.Scripts;

var builder = ServiceBuilder.GetBuilder(args);


switch (Environment.GetEnvironmentVariable("LAUNCH_MODE"))
{
    case "SERVER":
    {
        await Server.Run(builder);
        break;
    }
    case "LITRES_SCRIPT":
    {
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var parser = scope.ServiceProvider.GetRequiredService<LitResParser>();
            await parser.Download();
        }
        break;
    }
    case "HANGFIRE":
    {
        ServiceBuilder.AddHangfire(builder);
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var hangfire = scope.ServiceProvider.GetRequiredService<JobRunner>();
            await hangfire.Run(app);
        }
        break;
    }
}