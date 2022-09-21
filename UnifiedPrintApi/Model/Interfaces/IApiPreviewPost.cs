using System.Text.Json.Serialization;
using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiPreviewPost
{
    string Id { get; }
    string UniversalId => $"{Api.Slug}:{Id}";
    string Name { get; }
    GenericFile Thumbnail { get; }
    Uri Website { get; }
    IApiAuthor Author { get; }
    
    [JsonIgnore]
    IApiDescription Api { get; }
}