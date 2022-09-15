using UnifiedPrintApi.Model.Interfaces;

namespace UnifiedPrintApi.Model.Get;

public class SavedToken
{
    public List<IApiPost> Posts { get; set; } = new();
    public string Name { get; } = "";
}