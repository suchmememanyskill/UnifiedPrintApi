using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiAuthor
{
    string Name { get; }
    Uri Website { get; }
    GenericFile Thumbnail { get; }
}