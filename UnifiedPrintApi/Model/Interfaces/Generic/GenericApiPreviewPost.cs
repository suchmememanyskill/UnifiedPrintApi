using System.Text.Json.Serialization;

namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiPreviewPost : IApiPreviewPost
{
    public string Id { get; }
    public string Name { get; }
    public Uri Thumbnail { get; }
    public Uri Website { get; }
    public IApiAuthor Author { get; }
    [JsonIgnore]
    public IApiDescription Api { get; }

    public GenericApiPreviewPost(IApiPreviewPost post)
    {
        Id = post.Id;
        Name = post.Name;
        Thumbnail = post.Thumbnail;
        Website = post.Website;
        Author = post.Author.Generic();
        Api = post.Api;
    }
}