using UnifiedPrintApi.Model.Interfaces;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWRootFeatured
{
    public MWPagePropsFeatured PageProps { get; set; }
}

public class MWPagePropsFeatured : IApiPreviewPosts
{
    public List<FeaturedDesign> Designs { get; set; }

    public List<IApiPreviewPost> PreviewPosts => Designs.Select(x => (IApiPreviewPost)x.Design).ToList();
    public long TotalResults => Designs.Count;
}

public class FeaturedDesign
{
    public string PickReason { get; set; }
    public MWSearchHit Design { get; set; }
}