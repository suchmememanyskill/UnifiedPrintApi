namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiPost : IApiPreviewPost
{
    string Description { get; }
    List<Uri> Images { get; }
    List<Uri> Downloads { get; }
    DateTimeOffset Added { get; }
    DateTimeOffset Modified { get; }
    long DownloadCount { get; }
    long LikeCount { get; }
}