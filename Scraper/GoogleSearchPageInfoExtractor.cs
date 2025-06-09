using System.Text.RegularExpressions;

namespace InfoTrackProject;

public class GoogleSearchPageInfoExtractor
{
    public List<string> GetResultUrls(string pageHtml)
    {
        var targetRegex = new Regex("/url\\?q=(.*?)&sa=U&ved=");
        var matches = targetRegex.Matches(pageHtml);
        var matchList = matches.Select(x => x.Value).ToList();
        return matchList.Select(x => x.Remove(0, 7)).ToList();
    }
}