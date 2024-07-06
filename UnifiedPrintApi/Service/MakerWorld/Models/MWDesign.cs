using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWDesign : IApiPost
{
    [JsonProperty("id")]
    public long InternalId { get; set; }
    public string Title { get; set; }
    public Uri CoverUrl { get; set; }
    public string Summary { get; set; }
    public long DownloadCount { get; set; }
    public long LikeCount { get; set; }
    public MWDesignExtension DesignExtension { get; set; }
    public MWDesignCreator DesignCreator { get; set; }
    
    string IApiPreviewPost.Id => InternalId.ToString();
    public string Name => Title;
    public GenericFile Thumbnail => new(CoverUrl);
    public Uri Website => new($"https://makerworld.com/en/models/{InternalId}");
    public IApiAuthor Author => DesignCreator;
    [JsonIgnore]
    public IApiDescription Api { get; set; }

    public List<GenericFile> Images => DesignExtension.DesignPictures.Select(x => x.ToGenericFile()).ToList();

    public List<GenericFile> Downloads => DesignExtension.ModelFiles.Where(x => x.ModelUrl != null).Select(x => x.ToGenericFiles()).SelectMany(x => x).ToList();

    [JsonProperty("createTime")]
    public DateTimeOffset Added { get; set; }
    [JsonProperty("updateTime")]
    public DateTimeOffset Modified { get; set; }

    public string Description => Summary;
}