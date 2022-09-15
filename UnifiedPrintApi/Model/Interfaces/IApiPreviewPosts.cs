namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiPreviewPosts
{
    List<IApiPreviewPost> PreviewPosts { get; }
    long TotalResults { get; }
}