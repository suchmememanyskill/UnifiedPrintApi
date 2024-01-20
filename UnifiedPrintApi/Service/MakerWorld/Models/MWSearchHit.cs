using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWSearchHit : IApiPreviewPost
{
    [JsonProperty("id")]
    public long InternalId { get; set; }
    public string Title { get; set; }
    public Uri Cover { get; set; }
    public MWDesignCreator DesignCreator { get; set; }

    public string Id => InternalId.ToString();
    public string Name => Title;
    public GenericFile Thumbnail => new(Cover);
    public Uri Website => new($"https://makerworld.com/en/models/{InternalId}");
    public IApiAuthor Author => DesignCreator;
    [JsonIgnore]
    public IApiDescription Api { get; set; }
}