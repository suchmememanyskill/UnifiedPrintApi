using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Printables.Model;

namespace UnifiedPrintApi.Service.Printables;

public class PrintablesPost : IApiPost
{
    private PrintModel _data;
    private PrintablesApi _api;

    public PrintablesPost(PrintablesApi api, PrintModel data)
    {
        _data = data;
        _api = api;
    }


    public string Id => _data.Id;
    public string Name => _data.Name;
    public Uri Thumbnail => Images.First().Url;
    public Uri Website => _data.ToUri();
    public IApiAuthor Author => new PrintablesAuthor(_data.User);
    public IApiDescription Api => _api;
    public string Description => _data.Description; // TODO: De-Html-Ify
    public List<GenericFile> Images => _data.Images.Select(x => new GenericFile(x.Id, x.ToUri())).ToList(); // TODO: Return a sensible file name
    public List<GenericFile> Downloads => _data.Models.Select(x => new GenericFile(x.Name, x.ToUri())).ToList();
    public DateTimeOffset Added => _data.Published;
    public DateTimeOffset Modified => _data.Modified;
    public long DownloadCount => _data.DownloadCount;
    public long LikeCount => _data.LikesCount;
}