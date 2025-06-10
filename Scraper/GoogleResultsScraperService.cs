namespace InfoTrackProject;

public class GoogleResultsScraperService : IGoogleResultsScraperService
{
    private readonly IScraper _scraper;
    private readonly IGoogleSearchPageInfoExtractor _pageInfoExtractor;

    public GoogleResultsScraperService(IScraper scraper, IGoogleSearchPageInfoExtractor pageInfoExtractor)
    {
        _scraper = scraper;
        _pageInfoExtractor = pageInfoExtractor;
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