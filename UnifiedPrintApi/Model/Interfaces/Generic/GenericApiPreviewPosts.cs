namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiPreviewPosts : IApiPreviewPosts
{
    public List<IApiPreviewPost> PreviewPosts { get; set; }
    public long TotalResults { get; set; }

    public GenericApiPreviewPosts(IApiPreviewPosts posts)
    {
        PreviewPosts = posts.PreviewPosts.Select(x => (IApiPreviewPost)x.Generic()).ToList();
        TotalResults = posts.TotalResults;
    }

    public GenericApiPreviewPosts(IEnumerable<IApiPreviewPost> posts, long totalResults = -1)
    {
        PreviewPosts = posts.Select(x => (IApiPreviewPost)x.Generic()).ToList();
        TotalResults = totalResults;
    }
}