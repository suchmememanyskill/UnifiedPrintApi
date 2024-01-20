using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWDesignPicture
{
    public string Name { get; set; }
    public Uri Url { get; set; }

    public GenericFile ToGenericFile()
        => new(Url);
}