
using System.IO.Compression;
using System.Xml.Serialization;
using AdvMusicTest.DataLayer.Interfaces;
using AdvMusicTest.DataLayer.Types;
namespace AdvMusicTest.Scripts;



public class LitResParser
{
    private IOffersRepository _repository;
    private ILogger _logger;

    public LitResParser(IOffersRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }
    private async Task ProcessXML(Stream xmlStream)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LRCatalog));

        StreamReader reader = new StreamReader(xmlStream);
        var catalog = (LRCatalog)serializer.Deserialize(reader);
        reader.Close();

        await _repository.InsertRecords(catalog.Shop.Offers.ToList());
    }
    public async Task Download()
    {
        var tempFileName = System.IO.Path.GetTempFileName();
        var client = new HttpClient();

        _logger.LogInformation("A");

        HttpResponseMessage response = await client.GetAsync("https://www.litres.ru/static/ds/audio_all.xml.gz",
            HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("B");

        await using (var httpStream = await response.Content.ReadAsStreamAsync())
        await using (var fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite,
                         FileShare.None, 4096, FileOptions.DeleteOnClose | FileOptions.DeleteOnClose))
        {
            await httpStream.CopyToAsync(fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);
            await using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                await using (var resultStream = new MemoryStream())
                {
                    await gzStream.CopyToAsync(resultStream);
                    resultStream.Seek(0, SeekOrigin.Begin);
                    await ProcessXML(resultStream);
                }
            }
            httpStream.Close();
        }

        _logger.LogInformation($"Downloaded {tempFileName}");
    }
}