using UnifiedPrintApi.Model.Interfaces;

namespace UnifiedPrintApi.Model.Get;

public class SavedToken
{
    public string CollectionName { get; set; } = "";
    public List<IApiPost> Posts { get; set; } = new();
}