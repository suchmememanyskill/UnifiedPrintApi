namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericFullApiPost : IApiPost
{
    public string Id { get; set; }
    public string UniversalId { get; set; }
    public string Name { get; set; }
    public GenericFile Thumbnail { get; set; }
    public Uri Website { get; set; }
    public IApiAuthor Author => ActualAuthor;
    public IApiDescription Api => null;
    public GenericApiAuthor ActualAuthor { get; set; }
    public string Description { get; set; }
    public List<GenericFile> Images { get; set; }
    public List<GenericFile> Downloads { get; set; }
    public DateTimeOffset Added { get; set; }
    public DateTimeOffset Modified { get; set; }
    public long DownloadCount { get; set; }
    public long LikeCount { get; set; }
}