using System.Text.Json.Serialization;

namespace UnifiedPrintApi.Model;

public class SaveStorage
{
    public string Name { get; set; } = "";
    public List<string> UIDs { get; set; } = new();
}