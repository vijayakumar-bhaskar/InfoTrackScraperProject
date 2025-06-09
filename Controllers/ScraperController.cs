using InfoTrackProject;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackScraperProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ScraperController : ControllerBase
{
    private readonly GoogleResultsScraperService _googleScraperService;
    
    public ScraperController()
    {
        _googleScraperService = new GoogleResultsScraperService();
    }
    
    [HttpGet]
    [Route("search")]
    public IList<int> Get([FromQuery] string query, [FromQuery] string targetUrl)
    {
        var result = _googleScraperService.GetIndicesOfTheTarget(query, targetUrl);
        return result;
    }
}