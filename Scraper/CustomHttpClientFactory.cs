using System.Net;
using System.Text.Json;

namespace InfoTrackProject;

public class CustomHttpClientFactory
{

    public static async Task<T?> Resolve<T>(
        Func<HttpClient, Task<HttpResponseMessage>> clientMethod, 
        Func<HttpResponseMessage, Task<T>> responseMethod,
        Action<HttpRequestException>? exceptionHandler = null)
    {
        try
        {
            using var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = false;
            httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using var httpClient = new HttpClient(httpClientHandler);
            AddHeaders(httpClient);
            var response = await clientMethod(httpClient);
            response.EnsureSuccessStatusCode();
            return await responseMethod(response);
        }
        catch (HttpRequestException e)
        {
            exceptionHandler?.Invoke(e);
            return default;
        }
    }
    
    private static void AddHeaders(HttpClient client)
    {
        var headers = GetHeaders();
        foreach (var (attrib, value) in headers)
        {
            client.DefaultRequestHeaders.Add(attrib, value);
        }
    }

    private static Dictionary<string, string> GetHeaders()
    {
        var fileContent = File.ReadAllText("./Resources/headers.json");
        var headerMappings = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
        if (headerMappings == null || headerMappings.Count == 0)
        {
            throw new Exception("headers.json is empty!");
        }

        if (headerMappings["cookie"] == null)
        {
            throw new Exception("cookie is not set in headers.json!");       
        }
        // add consent to cookie
        headerMappings["cookie"] = $"CONSENT=YES+gb{headerMappings["cookie"]}";
        return headerMappings;
    }
}