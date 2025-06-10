using InfoTrackProject;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackScraperProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ScraperController : ControllerBase
{
    private readonly IGoogleResultsScraperService _googleScraperService;
    
    public ScraperController(IGoogleResultsScraperService googleScraperService)
    {
        _googleScraperService = googleScraperService;
    }
    
    [HttpGet]
    [Route("search")]
    public IList<int> Get([FromQuery] string query, [FromQuery] string targetUrl)
    {
        var result = _googleScraperService.GetIndicesOfTheTarget(query, targetUrl);
        return result;
    }
}