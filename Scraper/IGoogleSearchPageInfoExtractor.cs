namespace InfoTrackProject;

public interface IGoogleSearchPageInfoExtractor
{
    public List<string> GetResultUrls(string pageHtml);
}