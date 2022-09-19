using System.Text.Json;
using AdvMusicTest.DataLayer.AbstractClasses;
using AdvMusicTest.DataLayer.Interfaces;
using AdvMusicTest.DataLayer.Types;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AdvMusicTest.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;
    private readonly IOffersRepository _repository;

    public SearchController(ILogger<SearchController> logger, IOffersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [SwaggerOperation(Summary = "Search books.", Tags = new[] {"Search"})]
    [HttpGet]
    public async Task<List<LROffer>> Get(
        [FromQuery, SwaggerParameter("Search keyword.", Required = true)]
        string searchString
    )
    {
        return await _repository.GetOffers(searchString);
    }
}