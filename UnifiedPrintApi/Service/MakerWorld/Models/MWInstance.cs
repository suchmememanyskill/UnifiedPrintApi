using Newtonsoft.Json;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWInstance
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("profileId")]
    public string ProfileId { get; set; }
}