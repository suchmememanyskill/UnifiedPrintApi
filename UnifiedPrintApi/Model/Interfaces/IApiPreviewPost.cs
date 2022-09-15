using System.Text.Json.Serialization;

namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiPreviewPost
{
    string Id { get; }
    string UniversalId => $"{Api.Slug}:{Id}";
    string Name { get; }
    Uri Thumbnail { get; }
    Uri Website { get; }
    IApiAuthor Author { get; }
    
    [JsonIgnore]
    IApiDescription Api { get; }
}