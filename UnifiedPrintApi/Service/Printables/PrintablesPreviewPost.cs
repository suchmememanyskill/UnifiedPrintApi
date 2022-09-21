using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Printables.Model;

namespace UnifiedPrintApi.Service.Printables;

public class PrintablesPreviewPost : IApiPreviewPost
{
    private PrintablesItem _data;
    private PrintablesApi _api;

    public PrintablesPreviewPost(PrintablesApi api, PrintablesItem data)
    {
        _api = api;
        _data = data;
    }

    public string Id => _data.Id;
    public string Name => _data.Name;
    public GenericFile Thumbnail => new(_data.Images.First().ToUri());
    public Uri Website => _data.ToUri();
    public IApiAuthor Author => new PrintablesAuthor(_data.User);
    public IApiDescription Api => _api;
}