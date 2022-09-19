using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AdvMusicTest.DataLayer.AbstractClasses;
using AdvMusicTest.DataLayer.Interfaces;
using AdvMusicTest.DataLayer.Types;

namespace AdvMusicTest.DataLayer.Repositories;

public class OffersRepository : IOffersRepository
{
    private const String Index = "offers";

    private ElasticHttpClient _client;
    private ILogger _logger;

    public OffersRepository(ElasticHttpClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InsertRecords(List<LROffer> offers)
    {
        const int BATCH = 1000;

        for (int i = 0; i < offers.Count() / BATCH + 1; i++)
        {
            var batch = offers.Skip(i * BATCH).Take(BATCH).ToList();
            var bulkString = ComputeBulkQuery(batch);
            var resp = await _client.PostAsync("_bulk",
                new StringContent(bulkString, Encoding.UTF8, "application/json"));
            resp.EnsureSuccessStatusCode();
            var percent = (100.0 * i * BATCH) / offers.Count();
            _logger.LogInformation($"{percent}% completed");
        }
    }

    public String ComputeBulkQuery(List<LROffer> offers)
    {
        var indexIt = (String index, String id) =>
            new
            {
                index = new
                {
                    _id = id,
                    _index = index
                }
            };
        var ser = offers
            .Select(v => new Object[] {indexIt(Index, v.LitresId), v})
            .SelectMany(v => v);

        return String.Join("\n", ser.Select(sr => JsonSerializer.Serialize(sr))) + "\n";
    }

    private class EsSearchResponseHitsHits
    {
        public LROffer _source { get; set; }
    }

    private class EsSearchResponseHits
    {
        public EsSearchResponseHitsHits[] hits { get; set; }
    }

    private class EsSearchResponse
    {
        public EsSearchResponseHits hits { get; set; }
    }

    class EsOperator
    {
        public String query { get; set; }
        [JsonPropertyName("operator")] public String _operator { get; set; }
    }

    public async Task<List<LROffer>> GetOffers(string query)
    {
        var searchString = JsonSerializer.Serialize(new
        {
            query = new
            {
                match = new
                {
                    Name = new EsOperator()
                    {
                        query = query,
                        _operator = "and"
                    }
                }
            },
            fields = new [] {".*"},
            track_total_hits = true,
            _source = new []{"*"}
        });
        _logger.LogInformation(searchString);
        var resp = await _client.PostAsync($"{Index}/_search",
            new StringContent(searchString, Encoding.UTF8, "application/json"));
        resp.EnsureSuccessStatusCode();
        var ret = JsonSerializer.Deserialize<EsSearchResponse>(await resp.Content.ReadAsStringAsync());
        return ret.hits.hits.Select(h => h._source).ToList();
    }
}