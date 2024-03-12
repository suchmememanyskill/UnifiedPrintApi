using Newtonsoft.Json;

namespace UnifiedPrintApi.Service.Printables.Model;

public class DownloadModelRoot
{
    [JsonProperty("data")]
    public DownloadModelRootIntermediate Data { get; set; }
}

public class DownloadModelRootIntermediate
{
    [JsonProperty("getDownloadLink")]
    public DownloadModel Data { get; set; }
}

public class DownloadModel
{
    [JsonProperty("ok")]
    public bool Ok { get; set; }
    
    [JsonProperty("output")]
    public DownloadModelOutput Output { get; set; }
}

public class DownloadModelOutput
{
    [JsonProperty("link")]
    public string Link { get; set; }
}