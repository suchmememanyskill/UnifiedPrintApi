namespace UnifiedPrintApi.Model.Interfaces;

public record SortType(string DisplayName, string UriName, string? DisplayDescription = null);

public interface IApiDescription
{
    public string Name { get; }
    public string Color { get; }
    public List<SortType> SortTypes { get; }
    public Uri Site { get; }
    public string Description { get; }
}