using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.MMF.Models;

namespace UnifiedPrintApi.Service.MMF;

public class MMFPreviewPost : IApiPreviewPost
{
    private MMFApi _api;
    private Hit _data;

    public MMFPreviewPost(MMFApi api, Hit data)
    {
        _api = api;
        _data = data;
    }

    public string Id => _data.Id;
    public string Name => _data.Name;
    public GenericFile Thumbnail => new(_data.ObjImg);
    public Uri Website => _data.AbsoluteUrl;
    public IApiAuthor Author => new MMFAuthor(_data);
    public IApiDescription Api => _api;
}