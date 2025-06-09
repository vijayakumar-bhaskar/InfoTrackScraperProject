namespace InfoTrackProject;

public class HttpClientService
{
    public static async Task<T> ProcessHttpGetAsync<T>(
        Uri uri,
        Func<HttpResponseMessage, Task<T>> successResolver,
        Func<Exception, T> exceptionResolver,
        Action<HttpClientHandler>? httpClientHandlerConfig = null,
        Action<HttpClient>? httpClientConfig = null)
    {
        try
        {
            using var clientHandler = new HttpClientHandler();
            httpClientHandlerConfig?.Invoke(clientHandler);
            using var client = new HttpClient(clientHandler);
            httpClientConfig?.Invoke(client);
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await successResolver(response);
        }
        catch (Exception e)
        {
            return exceptionResolver(e);
        }
    }
}