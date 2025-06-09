using InfoTrackProject;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackScraperProject.Controllers;

[ApiController]
[Route("[controller]/search")]
public class ScraperController : ControllerBase
{
    private readonly Scraper _scraper;
    
    public ScraperController()
    {
        _scraper = new Scraper();
    }
    
    [HttpGet]
    public IList<int> Get([FromQuery] string query, [FromQuery] string targetUrl)
    {
        var result = _scraper.ScrapeGoogleSearch(query, targetUrl).Result;
        return result;
    }
}