﻿using System.Web;
using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Thingiverse.Models;
using Utils;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiversePost : IApiPost
{
    private RequestSpecificThing _data;
    private ThingiverseApi _api;
    private List<RequestDownload>? _downloads;
    private List<RequestImage>? _images;
    private string _baseUrl;
    
    public ThingiversePost(ThingiverseApi api, RequestSpecificThing data)
    {
        _api = api;
        _data = data;
        _baseUrl = EnvironmentManager.BaseUrl;
    }

    public void GetDownloads()
    {
        string? response = _api.MakeRequest($"https://www.thingiverse.com/api/things/{_data.Id}/files", $"https://www.thingiverse.com/thing:{_data.Id}");

        if (response == null)
            throw new Exception("Request failed");
        
        _downloads = JsonConvert.DeserializeObject<List<RequestDownload>>(response);
    }

    public void GetImages()
    {
        string? response = _api.MakeRequest($"https://www.thingiverse.com/api/things/{_data.Id}/images", $"https://www.thingiverse.com/thing:{_data.Id}");
        
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
        new GenericFile(x.Name, new(x.Sizes.First(y => y.Type == "display" && y.SizeSize == "large").Url.AbsoluteUri.Replace("https://cdn.thingiverse.com/assets/", $"{_baseUrl}/thingiverse/download_img/")))).ToList();
    public List<GenericFile> Downloads => _downloads!.Select(x => new GenericFile(x.Name, 
        new Uri(x.PublicUrl.ToString().Replace("https://www.thingiverse.com/download:", $"{_baseUrl}/thingiverse/download/") + $"?filename={HttpUtility.UrlEncode(x.Name)}"))).ToList();
    public DateTimeOffset Added => _data.Added;
    public DateTimeOffset Modified => _data.Modified;
    public long DownloadCount => _data.DownloadCount;
    public long LikeCount => _data.LikeCount;
}