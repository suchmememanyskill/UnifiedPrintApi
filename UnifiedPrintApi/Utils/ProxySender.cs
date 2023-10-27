using System.Net;
using System.Net.Http.Headers;

namespace Utils;

public class ProxySender
{
    private string proxy;
    private NetworkCredential? credentials;
    
    public ProxySender(string proxy, NetworkCredential credentials = null)
    {
        this.proxy = proxy;
        this.credentials = credentials;
    }

    public static ProxySender GetDefault() => new("socks5://142.93.68.63:2434", new("vpn", "unlimited"));

    public async Task<string> Post(Uri uri, string body, NetworkCredential? credential = null)
    {
        Console.WriteLine($"Requesting {uri}");
        
        var handler = new HttpClientHandler()
        {
            Proxy = new WebProxy(new Uri(proxy)),
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        
        if (credentials != null)
            handler.Proxy.Credentials = credentials;

        if (credential != null)
            handler.Credentials = credential;

        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/119.0");
        
        client.Timeout = TimeSpan.FromSeconds(10);
        for (int i = 0; i < 3; i++)
        {
            try
            {
                var httpResponse = await client.PostAsync(uri, new StringContent(body, new MediaTypeHeaderValue("application/json")));
                httpResponse.EnsureSuccessStatusCode();
                string response = await httpResponse.Content.ReadAsStringAsync();
                if (response.Contains("API limited"))
                    throw new Exception($"Api limited: {response}");

                await Task.Delay(1000); // Don't spam the servers
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request failed! {e.Message}");
            }
        }

        throw new Exception($"Request failed after 3 retries");
    }
}