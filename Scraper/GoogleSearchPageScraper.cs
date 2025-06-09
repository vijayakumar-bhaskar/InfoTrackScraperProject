using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace InfoTrackProject;

public class GoogleSearchPageScraper
{
    private readonly IList<Regex> _invalidTagRegexes;
    
    public GoogleSearchPageScraper()
    {
        _invalidTagRegexes = new List<Regex>
        {
            new Regex(@"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", RegexOptions.IgnoreCase),     // Script tags
            new Regex(@"<noscript\b[^<]*(?:(?!<\/noscript>)<[^<]*)*<\/noscript>", RegexOptions.IgnoreCase),
            new Regex(@"<textarea\b[^<]*(?:(?!<\/textarea>)<[^<]*)*<\/textarea>", RegexOptions.IgnoreCase),
            new Regex(@"<form\b[^<]*(?:(?!<\/form>)<[^<]*)*<\/form>", RegexOptions.IgnoreCase),
            new Regex(@"<img\b[^>]*>", RegexOptions.IgnoreCase),
            new Regex(@"&nbsp;", RegexOptions.IgnoreCase),
            //new Regex(" "),
            new Regex(@"<style(.*?)>.*?</style>", RegexOptions.IgnoreCase),       // Style tags
            new Regex(@"<meta[^>]*>", RegexOptions.IgnoreCase),                   // Meta tags
            new Regex(@"<link[^>]*>", RegexOptions.IgnoreCase),                   // Link tags
            new Regex(@"<!--.*?-->", RegexOptions.IgnoreCase),                      // HTML comments
            new Regex(@"<!doctype[^>]*>", RegexOptions.IgnoreCase),                 // XML declaration
            //new Regex(@"\son\w+\s*=\s*(['""]).*?\1"),       // inline functions
            //new Regex(@"function(\s+\w+)?\s*\([^)]*\)\s*\{[^}]*\}"), // basic functions
        };
    }

    public List<string> GetResultUrls(string pageHtml)
    {
        var targetRegex = new Regex("href=\"/url\\?q=http([^\"])+\"");
        var matches = targetRegex.Matches(pageHtml);
        var matchList = matches.Select(x => x.Value).ToList();
        return matchList.Select(x => x.Remove(0, 7)).ToList();
        //var document = CleanHtmlAndParseXDocument(pageHtml);
        /*var elements = document.Root!.DescendantsAndSelf()
            .Where(IsValidSearchResultUrlElement).Select(x => x.Attribute("href")!.Value.Remove(0, 7)).ToList();
        
        return elements;
        
        bool IsValidSearchResultUrlElement(XElement element) =>
            element.HasAttributes && element.HasElements && element.Attribute("href")?.Value is { } url && url.StartsWith("/url?Q=http", StringComparison.InvariantCultureIgnoreCase);*/
    }

    /*private XDocument CleanHtmlAndParseXDocument(string pageHtml)
    {
        var cleansedHtml = RemoveInvalidTags(pageHtml);
        var document = XDocument.Parse(cleansedHtml);
        return document;
    }

    private string RemoveInvalidTags(string pageHtml) =>
        _invalidTagRegexes.Aggregate(pageHtml, (current, regex) => regex.Replace(current, " "));*/
}