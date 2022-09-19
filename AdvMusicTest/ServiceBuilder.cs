
using AdvMusicTest.DataLayer.Interfaces;
using AdvMusicTest.DataLayer.Repositories;
using AdvMusicTest.DataLayer.Transport;
using AdvMusicTest.Scripts;
using Hangfire;
using Hangfire.MemoryStorage;

namespace AdvMusicTest;

public class ServiceBuilder
{
    public static WebApplicationBuilder GetBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddTransient<ILogger, CustomLogger>();

        builder.Services.AddTransient<IOffersRepository, OffersRepository>();
        builder.Services.AddSingleton(ElasticHttpClientFactory.GetClient());
        
        builder.Services.AddTransient<LitResParser>();
        builder.Services.AddScoped<JobRunner>();
        
        return builder;
    }

    public static void AddHangfire(WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage()
        );
        builder.Services.AddHangfireServer();
    }
}