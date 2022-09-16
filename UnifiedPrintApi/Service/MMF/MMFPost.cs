using System.Net;
using System.Net.Http.Headers;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.MMF.Models;

namespace UnifiedPrintApi.Service.MMF;

public class MMFPost : IApiPost
{
    private FetchSpecificObject _data;
    private MMFApi _api;
    private string _baseUrl;

    public MMFPost(MMFApi api, FetchSpecificObject data, string baseUrl)
    {
        _api = api;
        _data = data;
        _baseUrl = baseUrl;
    }

    public string Id => _data.Id.ToString();
    public string Name => _data.Name;
    public Uri Thumbnail => Images.First().Url;
    public Uri Website => _data.Url;
    public IApiAuthor Author => new MMFAuthor(_data.Designer);
    public IApiDescription Api => _api;
    public string Description => _data.Description;
    public List<GenericFile> Images =>
        _data.Images.Select(x => new GenericFile(x.Id.ToString(), x.Standard.Url)).ToList();

    public List<GenericFile> Downloads => _data.Files.Items
        .Select(x => new GenericFile(x.Filename, new Uri($"{_baseUrl}/mmf/{Id}/download/{x.Filename}"))).ToList();
    public DateTimeOffset Added => _data.PublishedAt;
    public DateTimeOffset Modified => Added;
    public long DownloadCount => _data.Views;
    public long LikeCount => _data.Likes;

    public Stream Download(string fileName)
    {
        GenericFile? file = Downloads.Find(x => x.Name == fileName);

        if (file == null)
            throw new Exception("File not found");
        
        HttpClient client = new();
        client.DefaultRequestHeaders.Referrer = Website;
        return client.GetStreamAsync(new Uri($"https://www.myminifactory.com/download/{Id}?downloadfile={file.Name}")).GetAwaiter().GetResult();
    }
}