using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWDownload
{
    public string ModelName { get; set; }
    public int ModelSize { get; set; }
    public Uri ModelUrl { get; set; }
    public bool IsDir { get; set; }
    public List<MWDownload> Children { get; set; }

    public List<GenericFile> ToGenericFiles()
    {
        if (IsDir)
            return Children.Select(x => x.ToGenericFiles()).SelectMany(x => x).ToList();

        // TODO: Figure out how to deal with .3mf's
        if (ModelUrl.AbsoluteUri.EndsWith(".3mf"))
            return new();
        
        return new(){ new(ModelUrl)};
    }
}