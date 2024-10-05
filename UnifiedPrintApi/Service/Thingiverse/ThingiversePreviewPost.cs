using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Thingiverse.Models;
using Utils;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiversePreviewPost : IApiPreviewPost
{
    private Hit _hit;
    private ThingiverseApi _api;
    private string _baseUrl;
    public ThingiversePreviewPost(ThingiverseApi api, Hit hit)
    {
        _hit = hit;
        _api = api;
        _baseUrl = EnvironmentManager.BaseUrl;
    }

    public string Id => _hit.Id.ToString();
    public string Name => _hit.Name;
    public GenericFile Thumbnail => new(new(_hit.Thumbnail.AbsoluteUri.Replace("https://cdn.thingiverse.com/assets/", $"{_baseUrl}/thingiverse/download_img/")));
    public Uri Website => _hit.PublicUrl;
    public IApiAuthor Author => new ThingiverseAuthor(_hit.Creator);
    public IApiDescription Api => _api;
}