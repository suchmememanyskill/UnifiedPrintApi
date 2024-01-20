using Newtonsoft.Json;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWDesignExtension
{
    [JsonProperty("design_pictures")]
    public List<MWDesignPicture> DesignPictures { get; set; }
    
    [JsonProperty("model_files")]
    public List<MWDownload> ModelFiles { get; set; }
}