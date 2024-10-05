using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Thingiverse.Models;
using Utils;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiverseAuthor : IApiAuthor
{
    private Creator _creator;
    private string _baseUrl;

    public ThingiverseAuthor(Creator creator)
    {
        _creator = creator;
        _baseUrl = EnvironmentManager.BaseUrl;
    }

    public string Name => _creator?.Name ?? "INVALID";
    public Uri Website => _creator?.PublicUrl ?? new("https://www.thingiverse.com");
    public GenericFile Thumbnail => (_creator == null) ? null : new(new(_creator.Thumbnail.AbsoluteUri.Replace("https://cdn.thingiverse.com/assets/", $"{_baseUrl}/thingiverse/download_img/")));
}