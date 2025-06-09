using System.Net;
using System.Text.Json;
using System.Web;

namespace InfoTrackProject;

public class Scraper
{

    private static readonly List<string> _expectedHeaders =
    [
        "accept",
        "accept-language",
        "cache-control",
        "downlink",
        "priority",
        "rtt",
        "sec-ch-prefers-color-scheme",
        "sec-ch-ua",
        "sec-ch-ua-arch",
        "sec-ch-ua-bitness",
        "sec-ch-ua-form-factors",
        "sec-ch-ua-full-version",
        "sec-ch-ua-full-version-list",
        "sec-ch-ua-mobile",
        "sec-ch-ua-model",
        "sec-ch-ua-platform",
        "sec-ch-ua-platform-version",
        "sec-ch-ua-wow64",
        "sec-fetch-dest",
        "sec-fetch-mode",
        "sec-fetch-site",
        "sec-fetch-user",
        "upgrade-insecure-requests",
        "x-browser-channel",
        "x-browser-copyright",
        "x-browser-validation",
        "x-browser-year",
        "x-client-data",
        "cookie"
    ];

    public async Task<string> Scrape(string query)
    {
        query = query.Replace(" ", "+");
        var clientHeaders = GetHttpClientHeaders();
        var result = await HttpClientService.ProcessHttpGetAsync(
            new Uri($"https://www.google.co.uk/search?q={query}&num=100&hl=en&gl=uk"),
            async response => await response.Content.ReadAsStringAsync(),
            _ => null, // log instead
            clientHandler =>
            {
                clientHandler.AllowAutoRedirect = true;
                clientHandler.UseCookies = false;
                clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            },
            client =>
            {
                foreach (var (attrib, value) in clientHeaders)
                {
                    client.DefaultRequestHeaders.Add(attrib, value);
                }
            }
        );
        // html return will be in encoded form
        return result; //HttpUtility.HtmlDecode(result);
    }

    private static Dictionary<string, string> GetHttpClientHeaders()
    {
        var fileContent = File.ReadAllText("./Resources/headers.json");
        var headerMappings = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
        if (headerMappings == null || headerMappings.Count == 0)
        {
            throw new Exception("./Resources/headers.json has no content.");
        }
        
        foreach(var expectedHeader in _expectedHeaders)
        {
            if (!headerMappings.ContainsKey(expectedHeader) || headerMappings[expectedHeader] == null || headerMappings[expectedHeader].Trim().Length == 0)
            {
                throw new Exception($"./Resources/headers.json is missing header {expectedHeader}.");
            }
        }
        
        // add consent to cookie
        headerMappings["cookie"] = $"CONSENT=YES+gb{headerMappings["cookie"]}";
        return headerMappings;
    }
    
}