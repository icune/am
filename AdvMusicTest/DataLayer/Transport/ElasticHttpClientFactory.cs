using AdvMusicTest.DataLayer.AbstractClasses;

namespace AdvMusicTest.DataLayer.Transport;

public class ElasticHttpClientFactory
{
    public static ElasticHttpClient GetClient()
    {
        var client = new ElasticHttpClient();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ELASTIC_URL"));
        return client;
    }
}