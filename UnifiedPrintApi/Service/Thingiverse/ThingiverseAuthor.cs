using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.Thingiverse.Models;

namespace UnifiedPrintApi.Service.Thingiverse;

public class ThingiverseAuthor : IApiAuthor
{
    private Creator _creator;

    public ThingiverseAuthor(Creator creator)
    {
        _creator = creator;
    }

    public string Name => _creator.Name;
    public Uri Website => _creator.PublicUrl;
    public Uri Thumbnail => _creator.Thumbnail;
}