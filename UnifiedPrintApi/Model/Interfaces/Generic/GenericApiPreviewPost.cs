using Newtonsoft.Json;

namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiPreviewPost : IApiPreviewPost
{
    public string Id { get; set; }
    public string UniversalId { get; set; }
    public string Name { get; set; }
    public Uri Thumbnail { get; set; }
    public Uri Website { get; set; }
    [JsonIgnore] 
    public IApiAuthor Author => ActualAuthor;
    [System.Text.Json.Serialization.JsonIgnore]
    public GenericApiAuthor ActualAuthor { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [JsonIgnore]
    public IApiDescription Api => null!;

    public GenericApiPreviewPost(IApiPreviewPost post)
    {
        Id = post.Id;
        UniversalId = post.UniversalId;
        Name = post.Name;
        Thumbnail = post.Thumbnail;
        Website = post.Website;
        ActualAuthor = post.Author.Generic();
    }

    public GenericApiPreviewPost()
    {
    }
}