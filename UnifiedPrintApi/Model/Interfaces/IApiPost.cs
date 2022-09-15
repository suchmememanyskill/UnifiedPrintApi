using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiPost : IApiPreviewPost
{
    string Description { get; }
    List<GenericFile> Images { get; }
    List<GenericFile> Downloads { get; }
    DateTimeOffset Added { get; }
    DateTimeOffset Modified { get; }
    long DownloadCount { get; }
    long LikeCount { get; }
}