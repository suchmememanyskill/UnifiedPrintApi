namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiPreviewPost
{
    string Id { get; }
    string Name { get; }
    Uri Thumbnail { get; }
    Uri Website { get; }
    IApiAuthor Author { get; }
}