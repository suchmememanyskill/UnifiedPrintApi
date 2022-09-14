namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiPreviewPost : IApiPreviewPost
{
    public string Id { get; }
    public string Name { get; }
    public Uri Thumbnail { get; }
    public Uri Website { get; }
    public IApiAuthor Author { get; }

    public GenericApiPreviewPost(IApiPreviewPost post)
    {
        Id = post.Id;
        Name = post.Name;
        Thumbnail = post.Thumbnail;
        Website = post.Website;
        Author = post.Author.Generic();
    }
}