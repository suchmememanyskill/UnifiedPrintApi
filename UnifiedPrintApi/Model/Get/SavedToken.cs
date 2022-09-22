using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Model.Get;

public class SavedToken
{
    public string CollectionName { get; set; } = "";
    public List<GenericApiPost> Posts { get; set; } = new();
}