using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.Thingiverse.Models;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiversePreviewPost : IApiPreviewPost
{
    private Hit _hit;
    private ThingiverseApi _api;
    public ThingiversePreviewPost(ThingiverseApi api, Hit hit)
    {
        _hit = hit;
        _api = api;
    }

    public string Id => _hit.Id.ToString();
    public string Name => _hit.Name;
    public Uri Thumbnail => _hit.Thumbnail;
    public Uri Website => _hit.PublicUrl;
    public IApiAuthor Author => new ThingiverseAuthor(_hit.Creator);
    public IApiDescription Api => _api;
}