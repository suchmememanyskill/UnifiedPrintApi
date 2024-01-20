using UnifiedPrintApi.Model.Interfaces;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWSearch : IApiPreviewPosts
{
    public long Total { get; set; } = 0;
    public List<MWSearchHit> Hits { get; set; } = new();
    
    public List<IApiPreviewPost> PreviewPosts => Hits.Select(x => (IApiPreviewPost)x).ToList();
    public long TotalResults => Total;
}