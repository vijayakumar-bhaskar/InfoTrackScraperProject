namespace InfoTrackProject;

public class GoogleResultsScraperService
{
    private readonly Scraper _scraper;
    private readonly GoogleSearchPageInfoExtractor _pageInfoExtractor;

    public GoogleResultsScraperService()
    {
        _scraper = new Scraper();
        _pageInfoExtractor = new GoogleSearchPageInfoExtractor();
    }

    public IList<int> GetIndicesOfTheTarget(string query, string targetUrl)
    {
        var resultsPageHtml = _scraper.Scrape(query).GetAwaiter().GetResult();
        var allUrls = _pageInfoExtractor.GetResultUrls(resultsPageHtml);
        return allUrls.Select((x, i) => (Url: x, Index: i))
            .Where(x => x.Url.Contains(targetUrl, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Index)
            .ToList();
    }
}