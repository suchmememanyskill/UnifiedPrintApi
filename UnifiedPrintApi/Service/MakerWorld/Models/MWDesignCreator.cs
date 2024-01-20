using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWDesignCreator : IApiAuthor
{
    public long Uid { get; set; }
    public string Name { get; set; }
    public Uri Avatar { get; set; }

    public Uri Website => new($"https://makerworld.com/en/u/{Uid}");
    public GenericFile Thumbnail => new(Avatar);
}