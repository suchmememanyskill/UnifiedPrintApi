namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiPost : GenericApiPreviewPost, IApiPost
{
    public string Description { get; set; }
    public List<GenericFile> Images { get; set; }
    public List<GenericFile> Downloads { get; set; }
    public DateTimeOffset Added { get; set; }
    public DateTimeOffset Modified { get; set; }
    public long DownloadCount { get; set; }
    public long LikeCount { get; set; }

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

    public GenericApiPost()
    {
    }
}