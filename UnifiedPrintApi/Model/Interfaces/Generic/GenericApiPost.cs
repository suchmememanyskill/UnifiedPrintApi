namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiPost : GenericApiPreviewPost, IApiPost
{
    public string Description { get; }
    public List<Uri> Images { get; }
    public List<Uri> Downloads { get; }
    public DateTimeOffset Added { get; }
    public DateTimeOffset Modified { get; }
    public long DownloadCount { get; }
    public long LikeCount { get; }

    public GenericApiPost(IApiPost post)
        : base(post)
    {
        Description = post.Description;
        Images = post.Images;
        Downloads = post.Downloads;
        Added = post.Added;
        Modified = post.Modified;
        DownloadCount = post.DownloadCount;
        LikeCount = post.LikeCount;
    }
}