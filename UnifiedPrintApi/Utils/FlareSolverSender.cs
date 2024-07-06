using Newtonsoft.Json;

namespace Utils;

public class FlareSolverRequest
{
    [JsonProperty("cmd")] 
    public string Cmd { get; set; } = "request.get";
    [JsonProperty("url")]
    public string Url { get; set; }
    [JsonProperty("maxTimeout")] 
    public int MaxTimeout { get; set; } = 10000;
}

public class FlareSolverResponse
{
    private static readonly List<int> _validStatusCodes = new List<int>()
    {
        0, 200
    };
    
    [JsonProperty("solution")]
    public FlareSolverResponseSolution Solution { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }

    public void EnsureSuccessfulRequest()
    {
        if (Status != "ok" || !_validStatusCodes.Contains(Solution.Status))
        {
            throw new Exception("Request failed");
        }
    }

    public string Body => Solution.Body;
}

public class FlareSolverResponseSolution
{
    [JsonProperty("response")]
    public string Body { get; set; }
    
    [JsonProperty("status")]
    public int Status { get; set; }
}

public class FlareSolverSender
{
    private string _flareSolverUri;

    public FlareSolverSender()
    {
        _flareSolverUri = Environment.GetEnvironmentVariable("FLARESOLVERR_URL");
        _flareSolverUri ??= "http://localhost:8191/v1";
    }

    public string Get(Uri uri)
        => Get(uri.AbsoluteUri);
    
    public string Get(string uri)
    {
        using var client = new HttpClient();
        var data = new FlareSolverRequest()
        {
            Url = uri
        };

        var flareResponse = client
            .PostAsync(_flareSolverUri, new StringContent(JsonConvert.SerializeObject(data), null, "application/json")).GetAwaiter()
            .GetResult();

        flareResponse.EnsureSuccessStatusCode();
        var parsedFlareResponse =
            JsonConvert.DeserializeObject<FlareSolverResponse>(flareResponse.Content.ReadAsStringAsync().GetAwaiter()
                .GetResult());

        parsedFlareResponse!.EnsureSuccessfulRequest();

        return parsedFlareResponse.Body
            .Replace(
                "<html><head><meta name=\"color-scheme\" content=\"light dark\"></head><body><pre style=\"word-wrap: break-word; white-space: pre-wrap;\">",
                "").Replace("</pre></body></html>", "");
    }
}