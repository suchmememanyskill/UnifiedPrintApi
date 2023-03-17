using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Thingiverse.Models;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiverseAuthor : IApiAuthor
{
    private Creator _creator;

    public ThingiverseAuthor(Creator creator)
    {
        _creator = creator;
    }

    public string Name => _creator?.Name ?? "INVALID";
    public Uri Website => _creator?.PublicUrl ?? new("https://www.thingiverse.com");
    public GenericFile Thumbnail => (_creator == null) ? null : new(_creator.Thumbnail);
}