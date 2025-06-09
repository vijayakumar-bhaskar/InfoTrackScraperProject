namespace InfoTrackProject;

public class Scraper
{
    private readonly GoogleSearchPageScraper _googleSearchPageScraper;
    
    public Scraper()
    {
        _googleSearchPageScraper = new GoogleSearchPageScraper();
    }
    
    public async Task<List<int>> ScrapeGoogleSearch(string query, string targetUrl)
    {
        var webPageString = await CustomHttpClientFactory.Resolve(
            async client => await client.GetAsync($"https://www.google.com/search?q={query}&num=100&hl=en&gl=uk"),
            async response => await response.Content.ReadAsStringAsync()
            );
        GetIndicesOfTargetUrl(webPageString, targetUrl);
        return GetIndicesOfTargetUrl(webPageString, targetUrl);
    }
    
    private List<int> GetIndicesOfTargetUrl(string webPageString, string targetUrl)
    {
        return _googleSearchPageScraper
            .GetResultUrls(webPageString)
            .Select((x, index) => (Url: x, Index: index))
            .Where(x => x.Url.Contains(targetUrl, StringComparison.InvariantCultureIgnoreCase))
            .Select(x => x.Index)
            .ToList();
    }
}