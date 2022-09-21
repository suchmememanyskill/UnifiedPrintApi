namespace UnifiedPrintApi.Model.Interfaces;

public record SortType(string DisplayName, string UriName, string? DisplayDescription = null);

public interface IApiDescription
{
    public string Name { get; }
    public string Slug => Name.ToLower().Replace(' ', '-');
    public string Color { get; }
    public List<SortType> SortTypes { get; }
    public Uri Site { get; }
    public string Description { get; }

    public IApiPreviewPosts GetPosts(SortType type, int page, int perPage);
    public IApiPreviewPosts GetPostsBySearch(string search, int page, int perPage);
    public IApiPost? GetPostById(string id);
}