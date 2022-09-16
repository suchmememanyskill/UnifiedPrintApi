using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.MMF.Models;

namespace UnifiedPrintApi.Service.MMF;

public class MMFAuthor : IApiAuthor
{
    public MMFAuthor(Hit hit)
    {
        Name = hit.UserName;
        Website = hit.UserUrl;
        Thumbnail = hit.UserImg;
    }

    public MMFAuthor(Designer designer)
    {
        Name = designer.Username;
        Website = designer.ProfileUrl;
        Thumbnail = designer.AvatarThumbnailUrl;
    }
    
    public string Name { get; }
    public Uri Website { get; }
    public Uri Thumbnail { get; }
}