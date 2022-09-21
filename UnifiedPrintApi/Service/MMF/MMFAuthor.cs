using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.MMF.Models;

namespace UnifiedPrintApi.Service.MMF;

public class MMFAuthor : IApiAuthor
{
    public MMFAuthor(Hit hit)
    {
        Name = hit.UserName;
        Website = hit.UserUrl;
        Thumbnail = new(hit.UserImg);
    }

    public MMFAuthor(Designer designer)
    {
        Name = designer.Username;
        Website = designer.ProfileUrl;
        Thumbnail = new(designer.AvatarThumbnailUrl);
    }
    
    public string Name { get; }
    public Uri Website { get; }
    public GenericFile Thumbnail { get; }
}