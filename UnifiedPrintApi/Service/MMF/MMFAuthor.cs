using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.MMF.Models;

namespace UnifiedPrintApi.Service.MMF;

public class MMFAuthor : IApiAuthor
{
    private Hit _hit;

    public MMFAuthor(Hit hit) => _hit = hit;


    public string Name => _hit.UserName;
    public Uri Website => _hit.UserUrl;
    public Uri Thumbnail => _hit.UserImg;
}