namespace InfoTrackProject;

public interface IGoogleResultsScraperService
{
    public IList<int> GetIndicesOfTheTarget(string query, string targetUrl);
}