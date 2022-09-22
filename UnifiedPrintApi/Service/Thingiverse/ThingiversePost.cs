using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Thingiverse.Models;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiversePost : IApiPost
{
    private RequestSpecificThing _data;
    private ThingiverseApi _api;
    private List<RequestDownload>? _downloads;
    private List<RequestImage>? _images;
    
    public ThingiversePost(ThingiverseApi api, RequestSpecificThing data)
    {
        _api = api;
        _data = data;
    }

    public void GetDownloads()
    {
        string? response = _api.MakeRequest(_data.FilesUrl.AbsoluteUri);

        if (response == null)
            throw new Exception("Request failed");
        
        _downloads = JsonConvert.DeserializeObject<List<RequestDownload>>(response);
    }

    public void GetImages()
    {
        string? response = _api.MakeRequest(_data.ImagesUrl.AbsoluteUri);
        
        if (response == null)
            throw new Exception("Request failed");

        _images = JsonConvert.DeserializeObject<List<RequestImage>>(response);
    }

    public string Id => _data.Id.ToString();
    public string Name => _data.Name;
    public GenericFile Thumbnail => new(_data.Thumbnail); // TODO: Find a better way to do this
    public Uri Website => _data.PublicUrl;
    public IApiAuthor Author => new ThingiverseAuthor(_data.Creator);
    public IApiDescription Api => _api;
    public string Description => _data.Description;

    public List<GenericFile> Images => _images!.Select(x =>
        new GenericFile(x.Name, x.Sizes.First(y => y.Type == "display" && y.SizeSize == "large").Url)).ToList();
    public List<GenericFile> Downloads => _downloads!.Select(x => new GenericFile(x.Name, x.PublicUrl)).ToList();
    public DateTimeOffset Added => _data.Added;
    public DateTimeOffset Modified => _data.Modified;
    public long DownloadCount => _data.DownloadCount;
    public long LikeCount => _data.LikeCount;
}